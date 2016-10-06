using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DynamicModelMigrator.Test
{
    [TestClass]
    public class Tests
    {
        private static string TEST_DATA_SOURCE = "Data Source=.;Initial Catalog=dmm_test;Integrated Security=True;Connect Timeout=30;";

        [TestCleanup]
        public void Cleanup()
        {
            TestHelper.RemoveDatabase(TEST_DATA_SOURCE, "dmm_test");
        }

        [TestMethod]
        public void ShouldThrowErrorForNullConnectionString()
        {
            Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => { return DynamicModelMigrator.Migrate<TestClass>(null); });
        }

        [TestMethod]
        public void ShouldThrowErrorForNullInitialCatalog()
        {
            Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => { return DynamicModelMigrator.Migrate<TestClass>("something"); });
        }


        [TestMethod]
        public void ShouldCreateDatabase()
        {
            DynamicModelMigrator.Migrate<TestClass>(TEST_DATA_SOURCE).Wait();
            var exists = DynamicModelMigrator.DatabaseExists(new System.Data.SqlClient.SqlConnectionStringBuilder(TEST_DATA_SOURCE)).Result;
            Xunit.Assert.True(exists);
        }

        [TestMethod]
        public void ShouldCreateTable()
        {
            DynamicModelMigrator.Migrate<TestClass>(TEST_DATA_SOURCE).Wait();
            var exists = DynamicModelMigrator.TableExists(new System.Data.SqlClient.SqlConnectionStringBuilder(TEST_DATA_SOURCE), "TestClass").Result;
            Xunit.Assert.True(exists);
        }

        [TestMethod]
        public void ShouldCreateAndThenMigrateTable()
        {
            DynamicModelMigrator.Migrate<TestClass>(TEST_DATA_SOURCE, "TESTCLASS").Wait();
            var exists = DynamicModelMigrator.TableExists(new System.Data.SqlClient.SqlConnectionStringBuilder(TEST_DATA_SOURCE), "TestClass").Result;
            Xunit.Assert.True(exists);
            DynamicModelMigrator.Migrate<MigratedTestClass>(TEST_DATA_SOURCE, "TESTCLASS").Wait();
            var matchesClass = TestHelper.DoesClassMatchType<MigratedTestClass>(TEST_DATA_SOURCE, "TESTCLASS").Result;
            Xunit.Assert.True(matchesClass);
        }
    }

    public class TestClass : ClassWithId
    {
        public string StringField { get; set; }
    }

    public class MigratedTestClass : ClassWithId
    {
        public string StringField { get; set; }
        public int IntegerField { get; set; }
        public double DoubleField { get; set; }
        public bool BooleanField { get; set; }
        public long LongField { get; set; }
    }

    public static class TestHelper
    {
        public static void RemoveDatabase(string connection, string db)
        {
            try
            {
                var sqlConnectionBuilder = new SqlConnectionStringBuilder(connection);
                sqlConnectionBuilder.InitialCatalog = "master";
                using (var conn = new SqlConnection(sqlConnectionBuilder.ToString()))
                {

                    var command = new SqlCommand($"ALTER DATABASE {db} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {db}; ", conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                // this will fail if the db doesn't exist
                // which is ok because we don't want it to exist
            }
            
        }

        public async static Task<bool> DoesClassMatchType<T>(string conn, string tableName)
        {
            var columnMap = await DynamicModelMigrator.GetColumnMap(new SqlConnection(conn), tableName);
            var typeMap = DynamicModelMigrator.GetTypeMap<MigratedTestClass>();
            var columns = new List<string>();
            var typeProperties = new List<string>();

            foreach(var key in columnMap.Keys)
            {
                columns.Add(key);
            }

            foreach(var key in typeMap.Keys)
            {
                typeProperties.Add(key.Name);
            }

            var match = true;

            foreach(var column in columns)
            {
                if (!typeProperties.Contains(column))
                {
                    match = false;
                }
            }


            foreach(var typeProp in typeProperties)
            {
                if (!columns.Contains(typeProp))
                {
                    match = false;
                }
            }

            return match;
        }
    }
}
