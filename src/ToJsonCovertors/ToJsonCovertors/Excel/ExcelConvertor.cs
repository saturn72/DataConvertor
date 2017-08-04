#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using fastJSON;

#endregion

namespace ToJsonCovertors.Excel
{
    public class ExcelConvertor : IFileConvertor<ExcelConvertorSettings>
    {
        public DatasourceType ConvertsFrom => DatasourceType.Excel;

        public string ToJson(string filePath, ExcelConvertorSettings settings)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                return ToJson(stream, settings);
            }
        }

        public string ToJson(Stream stream, ExcelConvertorSettings settings)
        {
            settings = ProcessSettings(settings);

            var headers = new string[] { };
            var mandatoryColumnsIndexes = new int[] { };
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                //Get headers first;
                if (!reader.Read() || !HandleHeaders(settings, reader, ref headers, ref mandatoryColumnsIndexes))
                    throw new ArgumentException("First row in excel cannot be empty");

                var result = new List<IDictionary<string, string>>();

                var shouldCheckMandatoryColumns = mandatoryColumnsIndexes.Any();

                while (reader.Read())
                {
                    var rowCells = reader.GetCurrentRow();
                    if (IsEmptyRow(rowCells))
                        return ToJson(result);
                    if (shouldCheckMandatoryColumns)
                        foreach (var mci in mandatoryColumnsIndexes)
                            if (rowCells[mci] == null)
                                throw new ArgumentException(headers[mci] + " is required");
                    var curJsonNode = new Dictionary<string, string>();
                    for (var i = 0; i < headers.Length; i++)
                        curJsonNode.Add(headers[i], rowCells[i]?.ToString());
                    result.Add(curJsonNode);
                }
                return JSON.ToJSON(result);
            }
        }

        private static string ToJson(IEnumerable<IDictionary<string, string>> json)
        {
            return JSON.ToJSON(json);
        }

        private static ExcelConvertorSettings ProcessSettings(ExcelConvertorSettings settings)
        {
            if (settings == null)
                return new ExcelConvertorSettings();

            var newMandatoryColumns = new List<string>();
            if (settings.MandatoryColumns.Any())
            {
                foreach (var smc in settings.MandatoryColumns)
                    if (!string.IsNullOrWhiteSpace(smc) && !string.IsNullOrEmpty(smc))
                        newMandatoryColumns.Add(smc);

                settings.MandatoryColumns = JsonCommon.ToCamelCase(newMandatoryColumns);
            }
            return settings;
        }

        private static bool IsEmptyRow(IEnumerable<object> rowCells)
        {
            return rowCells.All(x => string.IsNullOrWhiteSpace(x?.ToString()) || string.IsNullOrEmpty(x.ToString()));
        }

        private static bool HandleHeaders(ExcelConvertorSettings settings, IDataRecord reader, ref string[] headers,
            ref int[] mandatoryColumnsIndexes)
        {
            if (reader.FieldCount == 0 || reader.RowHasEmptyCells())
                return false;

            var currentRow = reader.GetCurrentRow().Select(s=>JsonCommon.StringCleanup(s.ToString()));

            var tmpHeader = JsonCommon.ToCamelCase(currentRow).ToArray();
            
            var mci = new List<int>();
            for (var i = 0; i < tmpHeader.Count(); i++)
                if (settings.MandatoryColumns.Any(s => tmpHeader[i]
                    .Equals(s, StringComparison.InvariantCultureIgnoreCase)))
                    mci.Add(i);

            mandatoryColumnsIndexes = mci.ToArray();
            
            headers = tmpHeader;
            return true;
        }
    }
}