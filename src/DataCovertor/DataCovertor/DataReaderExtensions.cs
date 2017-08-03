#region Usings

using System.Data;

#endregion

namespace DataCovertor
{
    public static class DataReaderExtensions
    {
        internal static object[] GetCurrentRow(this IDataRecord reader)
        {
            var res = new object[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
                res[i] = reader.GetValue(i);
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