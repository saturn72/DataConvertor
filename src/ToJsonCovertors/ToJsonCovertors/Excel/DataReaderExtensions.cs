#region Usings

using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace ToJsonCovertors.Excel
{
    public static class DataReaderExtensions
    {
        private static readonly IDictionary<Type, Func<IDataRecord, int, object>> ExtractDictionary =
            new Dictionary<Type, Func<IDataRecord, int, object>>
            {
                {typeof(bool), (reader, index) => reader.GetBoolean(index)},
                {typeof(byte), (reader, index) => reader.GetByte(index)},
                {typeof(char), (reader, index) => reader.GetChar(index)},
                {typeof(DateTime), (reader, index) => reader.GetDateTime(index)},
                {typeof(decimal), (reader, index) => reader.GetDecimal(index)},
                {typeof(double), (reader, index) => reader.GetDouble(index)},
                {typeof(float), (reader, index) => reader.GetFloat(index)},
                {typeof(Guid), (reader, index) => reader.GetGuid(index)},
                {typeof(short), (reader, index) => reader.GetInt16(index)},
                {typeof(int), (reader, index) => reader.GetInt32(index)},
                {typeof(long), (reader, index) => reader.GetInt64(index)},
                {typeof(string), (reader, index) => reader.GetString(index)}
            };

        internal static object[] GetCurrentRow(this IDataRecord reader)
        {
            var res = new object[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var fieldType = reader.GetFieldType(i);
                res[i] = fieldType == null ? null : ExtractDictionary[fieldType](reader, i);
            }
            return res;
        }

        internal static bool RowCellIsEmpty(this IDataRecord reader, int index)
        {
            var value = reader.GetValue(index);
            return string.IsNullOrEmpty(value?.ToString()) || string.IsNullOrWhiteSpace(value.ToString());
        }

        internal static bool RowHasEmptyCells(this IDataRecord reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
                if (reader.RowCellIsEmpty(i))
                    return true;
            return false;
        }
    }
}