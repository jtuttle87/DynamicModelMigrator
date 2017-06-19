using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Text;

namespace DynamicModelMigrator.Test
{
    [TestClass]
    public class CLRToSqlDbTypeMapperTests
    {
        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenBooleanType_ReturnsBitSqlDbType()
        {
            Type value = typeof(Boolean);
            const SqlDbType expectedSqlDbType = SqlDbType.Bit;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableBooleanType_ReturnsBitSqlDbType()
        {
            Type value = typeof(Boolean?);
            const SqlDbType expectedSqlDbType = SqlDbType.Bit;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenByteType_ReturnsTinyIntSqlDbType()
        {
            Type value = typeof(Byte);
            const SqlDbType expectedSqlDbType = SqlDbType.TinyInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableByteType_ReturnsTinyIntSqlDbType()
        {
            Type value = typeof(Byte?);
            const SqlDbType expectedSqlDbType = SqlDbType.TinyInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenStringType_ReturnsNVarCharSqlDbType()
        {
            Type value = typeof(String);
            const SqlDbType expectedSqlDbType = SqlDbType.NVarChar;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenDateTimeType_ReturnsDateTimeSqlDbType()
        {
            Type value = typeof(DateTime);
            const SqlDbType expectedSqlDbType = SqlDbType.DateTime;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableDateTimeType_ReturnsDateTimeSqlDbType()
        {
            Type value = typeof(DateTime?);
            const SqlDbType expectedSqlDbType = SqlDbType.DateTime;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenInt16Type_ReturnsSmallIntSqlDbType()
        {
            Type value = typeof(Int16);
            const SqlDbType expectedSqlDbType = SqlDbType.SmallInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableInt16Type_ReturnsSmallIntSqlDbType()
        {
            Type value = typeof(Int16?);
            const SqlDbType expectedSqlDbType = SqlDbType.SmallInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenInt32Type_ReturnsIntSqlDbType()
        {
            Type value = typeof(Int32);
            const SqlDbType expectedSqlDbType = SqlDbType.Int;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableInt32Type_ReturnsIntSqlDbType()
        {
            Type value = typeof(Int32?);
            const SqlDbType expectedSqlDbType = SqlDbType.Int;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenInt64Type_ReturnsBigIntSqlDbType()
        {
            Type value = typeof(Int64);
            const SqlDbType expectedSqlDbType = SqlDbType.BigInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableInt64Type_ReturnsBigIntSqlDbType()
        {
            Type value = typeof(Int64?);
            const SqlDbType expectedSqlDbType = SqlDbType.BigInt;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenDecimalType_ReturnsDecimalSqlDbType()
        {
            Type value = typeof(Decimal);
            const SqlDbType expectedSqlDbType = SqlDbType.Decimal;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableDecimalType_ReturnsDecimalSqlDbType()
        {
            Type value = typeof(Decimal?);
            const SqlDbType expectedSqlDbType = SqlDbType.Decimal;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenDoubleType_ReturnsFloatSqlDbType()
        {
            Type value = typeof(Double);
            const SqlDbType expectedSqlDbType = SqlDbType.Float;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableDoubleType_ReturnsFloatSqlDbType()
        {
            Type value = typeof(Double?);
            const SqlDbType expectedSqlDbType = SqlDbType.Float;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenSingleType_ReturnsRealSqlDbType()
        {
            Type value = typeof(Single);
            const SqlDbType expectedSqlDbType = SqlDbType.Real;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableSingleType_ReturnsRealSqlDbType()
        {
            Type value = typeof(Single?);
            const SqlDbType expectedSqlDbType = SqlDbType.Real;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenTimeSpanType_ReturnsTimeSqlDbType()
        {
            Type value = typeof(TimeSpan);
            const SqlDbType expectedSqlDbType = SqlDbType.Time;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenGuidType_ReturnsUniqueIdentifierSqlDbType()
        {
            Type value = typeof(Guid);
            const SqlDbType expectedSqlDbType = SqlDbType.UniqueIdentifier;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableGuidType_ReturnsUniqueIdentifierSqlDbType()
        {
            Type value = typeof(Guid?);
            const SqlDbType expectedSqlDbType = SqlDbType.UniqueIdentifier;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenByteArrayType_ReturnsBinarySqlDbType()
        {
            Type value = typeof(Byte[]);
            const SqlDbType expectedSqlDbType = SqlDbType.Binary;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableByteArrayType_ReturnsBinarySqlDbType()
        {
            Type value = typeof(Byte?[]);
            const SqlDbType expectedSqlDbType = SqlDbType.Binary;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenCharArrayType_ReturnsCharSqlDbType()
        {
            Type value = typeof(Char[]);
            const SqlDbType expectedSqlDbType = SqlDbType.Char;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        public void GetSqlDbTypeFromClrType_WhenGivenNullableCharArrayType_ReturnsCharSqlDbType()
        {
            Type value = typeof(Char?[]);
            const SqlDbType expectedSqlDbType = SqlDbType.Char;
            SqlDbType actual = CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
            Assert.AreEqual(expectedSqlDbType, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetSqlDbTypeFromClrType_WhenGivenUnexpectedType_THEN()
        {
            Type value = typeof(StringBuilder);
            CLRToSqlDbTypeMapper.GetSqlDbTypeFromClrType(value);
        }
    }
}
