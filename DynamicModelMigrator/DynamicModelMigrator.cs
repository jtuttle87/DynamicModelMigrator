using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicModelMigrator
{
    public abstract class ClassWithId
    {
        public int Id { get; set; }
    }

    public static class DynamicModelMigrator
    {
        
        public async static Task Migrate<T>(string connectionString, string tableName = null) where T: ClassWithId
        {

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            var connection = new SqlConnectionStringBuilder(connectionString);

            if (string.IsNullOrWhiteSpace(connection.InitialCatalog))
            {
                throw new ArgumentNullException("initialCatalog");
            }

            await CreateDatabase(connection);

            // if name not provided then use the name of the type
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = typeof(T).Name;
            }

            await CreateTable(connection, tableName);

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var columnMap = await GetColumnMap(conn, tableName);
                var typeMap = GetTypeMap<T>();

                // add columns that don't exist
                var missingColumns = typeMap.Keys.Where(k => columnMap.Keys.Contains(k.Name) == false);
                foreach(var column in missingColumns)
                {
                    var isNullable = Nullable.GetUnderlyingType(typeMap[column]) != null;
                    var nullText = isNullable ? "NULL" : "NOT NULL";
                    var attributes = typeof(T)
                                       .GetProperty(column.Name)
                                       .GetCustomAttributes(false)
                                       .ToDictionary(a => a.GetType().Name, a => a);
                    var stringLengthAttribute = attributes.Any(x => x.Key == "StringLengthAttribute") ? attributes["StringLengthAttribute"] as StringLengthAttribute : null;
                    var lengthText = string.Empty;
                    if(stringLengthAttribute != null)
                    {
                        lengthText = $"({stringLengthAttribute.Length})";
                    }
                    var sql = $"ALTER TABLE {tableName} ADD {column.Name} {CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(typeMap[column])}{lengthText} {nullText}";
                    var alterCmd = new SqlCommand(sql, conn);
                    alterCmd.ExecuteNonQuery();
                }

                // remove columns that shouldn't exists
                var columnsToRemove = columnMap.Keys.Where(x => typeMap.Keys.All(tm => tm.Name != x));
                foreach (var columnToRemove in columnsToRemove)
                {
                    var sql = $"ALTER TABLE {tableName} DROP COLUMN {columnToRemove}";
                    var alterCmd = new SqlCommand(sql, conn);
                    alterCmd.ExecuteNonQuery();
                }

                // migrate columns where the datatype has changed
                var columnsToMigrate = GetColumnsToMigrate(columnMap, typeMap);
                foreach(var columnToMigrate in columnsToMigrate)
                {
                    // if this fails then the conversion between types was not possible
                    try
                    {
                        var sql = $"ALTER TABLE {tableName} ALTER COLUMN {columnToMigrate.Key} {CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(columnToMigrate.Value)}";
                        var alterCmd = new SqlCommand(sql, conn);
                        alterCmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        // so rename
                        var sql = $"exec sp_rename '[{tableName}].[{columnToMigrate.Key}]', 'TEMP_{columnToMigrate.Key}_TEMP', 'COLUMN';";
                        var renameCmd = new SqlCommand(sql, conn);
                        renameCmd.ExecuteNonQuery();

                        // add column
                        sql = $"ALTER TABLE {tableName} ADD {columnToMigrate.Key} {CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(columnToMigrate.Value)}";
                        var alterCmd = new SqlCommand(sql, conn);
                        alterCmd.ExecuteNonQuery();

                        // attempt conversion, if this fails than eff it
                        try
                        {
                            sql = $"UPDATE {tableName} SET {columnToMigrate.Key} = convert({CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(columnToMigrate.Value)}, TEMP_{columnToMigrate.Key}_TEMP) WHERE TEMP_{columnToMigrate.Key}_TEMP is not null";
                            var updateCmd = new SqlCommand(sql, conn);
                            updateCmd.ExecuteNonQuery();
                        }
                        catch(Exception exception)
                        {

                        }
                   

                        // drop temp
                        sql = $"ALTER TABLE {tableName} DROP COLUMN TEMP_{columnToMigrate.Key}_TEMP";
                        var dropcmd = new SqlCommand(sql, conn);
                        dropcmd.ExecuteNonQuery();
                    }
                }

                conn.Close();
            }
        }

        private static Dictionary<string, Type> GetColumnsToMigrate(Dictionary<string, Type> columnMap, Dictionary<System.Reflection.PropertyInfo, Type> typeMap)
        {
            var columnsToMigrate = new Dictionary<string, Type>();
            foreach (var columnName in columnMap.Keys)
            {
                var typeColumn = typeMap.Keys.FirstOrDefault(x => x.Name == columnName);
                if (typeColumn != null && typeMap[typeColumn] != columnMap[columnName])
                {
                    columnsToMigrate[columnName] = typeMap[typeColumn];
                }
            }

            return columnsToMigrate;
        }

        public static Dictionary<System.Reflection.PropertyInfo,Type> GetTypeMap<T>()
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var typeMap = new Dictionary<System.Reflection.PropertyInfo, Type>();
            foreach (var prop in props)
            {
                typeMap.Add(prop, prop.PropertyType);
            }

            return typeMap;
        }

        public async static Task<Dictionary<string, Type>> GetColumnMap(SqlConnection conn, string tableName)
        {
            if (conn != null && conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var sql = $"SELECT * FROM {tableName} WHERE 1=2";
            var cmd = new SqlCommand(sql, conn);
            var reader = cmd.ExecuteReader();

            var columnMap = new Dictionary<string, Type>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                columnMap.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            reader.Close();
            return columnMap;
        }


        private static async Task CreateTable(SqlConnectionStringBuilder connection, string table)
        {
            var tableExists = await TableExists(connection, table);
            if (!tableExists)
            {
                using (var conn = new SqlConnection(connection.ToString()))
                {
                    await conn.OpenAsync();
                    var sql = $"CREATE TABLE {table} (Id int PRIMARY KEY IDENTITY(1,1))";
                    var cmd = new SqlCommand(sql, conn);
                    await cmd.ExecuteNonQueryAsync();
                }
                tableExists = await TableExists(connection, table);
                if (!tableExists)
                {
                    throw new Exception("unable to create table");
                }
            }
        }

        private static async Task CreateDatabase(SqlConnectionStringBuilder connection)
        {
            var dbExists = await DatabaseExists(connection);
            if (!dbExists)
            {
                var masterConnection = new SqlConnectionStringBuilder(connection.ToString());
                masterConnection.InitialCatalog = "master";
                using (var conn = new SqlConnection(masterConnection.ToString()))
                {
                    await conn.OpenAsync();
                    var sql = $"CREATE DATABASE {connection.InitialCatalog}";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                dbExists = await DatabaseExists(connection);
                if (!dbExists)
                {
                    throw new Exception("unable to create database");
                }
            }
        }

        public async static Task<bool> DatabaseExists(SqlConnectionStringBuilder connection)
        {
            var masterConnection = new SqlConnectionStringBuilder(connection.ToString());
            masterConnection.InitialCatalog = "master";
            using (var conn = new SqlConnection(masterConnection.ToString()))
            {
                using (var command = new SqlCommand($"SELECT * FROM sys.databases WHERE [name] = @name", conn))
                {
                    command.Parameters.AddWithValue("name", connection.InitialCatalog);
                    conn.Open();
                    var result = await command.ExecuteReaderAsync();
                    return result.HasRows;
                }
            }
        }

        public async static Task<bool> TableExists(SqlConnectionStringBuilder connection, string table)
        {
            using (var conn = new SqlConnection(connection.ToString()))
            {
                using (var command = new SqlCommand($"SELECT 1 FROM {table} WHERE 1=2", conn))
                {
                    conn.Open();
                    try
                    {
                        var result = await command.ExecuteReaderAsync();
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                    
                    return true;
                }
            }
        }

    }
}
