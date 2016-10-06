using System;
using System.Collections.Generic;
using System.Data;

namespace DynamicModelMigrator
{
    // basicly this: https://gist.github.com/dibley1973/94232025f636747e5347 with a few modificaitons
    public static class CLRToSqlDbTypeMapper
    {
        static CLRToSqlDbTypeMapper()
        {
            CreateMap();
        }
        public static SqlDbType GetSqlDbTypeFromClrType(Type clrType)
        {
            EnsureTypeExists(clrType);

            SqlDbType result;
            _map.TryGetValue(clrType, out result);
            return result;
        }


        private static void CreateMap()
        {
            _map = new Dictionary<Type, SqlDbType>
            {
                {typeof (Boolean), SqlDbType.Bit},
                {typeof (Boolean?), SqlDbType.Bit},
                {typeof (Byte), SqlDbType.TinyInt},
                {typeof (Byte?), SqlDbType.TinyInt},
                {typeof (String), SqlDbType.NVarChar},
                {typeof (DateTime), SqlDbType.DateTime},
                {typeof (DateTime?), SqlDbType.DateTime},
                {typeof (Int16), SqlDbType.SmallInt},
                {typeof (Int16?), SqlDbType.SmallInt},
                {typeof (Int32), SqlDbType.Int},
                {typeof (Int32?), SqlDbType.Int},
                {typeof (Int64), SqlDbType.BigInt},
                {typeof (Int64?), SqlDbType.BigInt},
                {typeof (Decimal), SqlDbType.Decimal},
                {typeof (Decimal?), SqlDbType.Decimal},
                {typeof (Double), SqlDbType.Float},
                {typeof (Double?), SqlDbType.Float},
                {typeof (Single), SqlDbType.Real},
                {typeof (Single?), SqlDbType.Real},
                {typeof (TimeSpan), SqlDbType.Time},
                {typeof (Guid), SqlDbType.UniqueIdentifier},
                {typeof (Guid?), SqlDbType.UniqueIdentifier},
                {typeof (Byte[]), SqlDbType.Binary},
                {typeof (Byte?[]), SqlDbType.Binary},
                {typeof (Char[]), SqlDbType.Char},
                {typeof (Char?[]), SqlDbType.Char}
            };
        }

        private static void EnsureTypeExists(Type clrType)
        {
            if (!_map.ContainsKey(clrType))
            {
                throw new Exception($"No mapped type found for {clrType.ToString()}");
            }
        }

        private static Dictionary<Type, SqlDbType> _map;
    }
}

