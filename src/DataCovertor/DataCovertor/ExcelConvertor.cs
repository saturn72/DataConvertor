#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ExcelDataReader;

#endregion

namespace DataCovertor
{
    public class ExcelConvertor : IFileConvertor<XDocument, ExcelConvertorSettings>
    {
        public DatasourceType ConvertsFrom => DatasourceType.Excel;

        public XDocument Convert(string filePath, ExcelConvertorSettings settings)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                return Convert(stream, settings);
            }
        }

        public XDocument Convert(Stream stream, ExcelConvertorSettings settings)
        {
            settings = ProcessSettings(settings);

            var headers = new string[] { };
            var mandatoryColumnsIndexes = new int[] { };
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                //Get headers first;
                if (!reader.Read() || !HandleHeaders(settings, reader, ref headers, ref mandatoryColumnsIndexes))
                    throw new ArgumentException("First row in excel cannot be empty");

                var result = new XDocument(new XElement(settings.XmlRootNodeName));
                var shouldCheckMandatoryColumns = mandatoryColumnsIndexes.Any();

                while (reader.Read())
                {
                    var rowCells = reader.GetCurrentRow();
                    if (IsEmptyRow(rowCells))
                        return result;
                    if (shouldCheckMandatoryColumns)
                        foreach (var mci in mandatoryColumnsIndexes)
                            if (rowCells[mci] == null)
                                throw new ArgumentException(headers[mci] + " is required");
                    var curElement = new XElement(settings.XmlIterativeNodeName);
                    for (var i = 0; i < headers.Length; i++)
                        curElement.Add(new XElement(headers[i], rowCells[i]));
                    result.Root.Add(curElement);
                }
                return result;
            }
        }

        private static ExcelConvertorSettings ProcessSettings(ExcelConvertorSettings settings)
        {
            if (settings == null)
                return ExcelConvertorSettings.Default;

            var newMandatoryColumns = new List<string>();
            if (settings.MandatoryColumns.Any())
            {
                foreach (var smc in settings.MandatoryColumns)
                    if (!string.IsNullOrWhiteSpace(smc) && !string.IsNullOrEmpty(smc))
                        newMandatoryColumns.Add(smc);

                settings.MandatoryColumns = ToCamelCase(newMandatoryColumns);
            }
            return settings;
        }

        private bool IsEmptyRow(IEnumerable<object> rowCells)
        {
            return rowCells.All(x => string.IsNullOrWhiteSpace(x?.ToString()) || string.IsNullOrEmpty(x.ToString()));
        }

        private bool HandleHeaders(ExcelConvertorSettings settings, IDataRecord reader, ref string[] headers,
            ref int[] mandatoryColumnsIndexes)
        {
            if (reader.FieldCount == 0 || reader.RowHasEmptyCells())
                return false;


            var tmpHeader = ToCamelCase(reader.GetCurrentRow()).ToArray();

            var mci = new List<int>();
            for (var i = 0; i < tmpHeader.Count(); i++)
                if (settings.MandatoryColumns.Any(s => tmpHeader[i]
                    .Equals(s, StringComparison.InvariantCultureIgnoreCase)))
                    mci.Add(i);

            mandatoryColumnsIndexes = mci.ToArray();
            var illegalXmlValues = new[]
            {
                "!", "“", "#", "$", "%",
                "&", "‘", "(", ")", "*",
                "+", ",", "-", ".", "/",
                ";", "<", "=", ">", "?",
                "@", "{", "|", "}", "~",
                "[", "\\", "]", "^", "*"
            };
            for (int i = 0; i < tmpHeader.Length; i++)
            {
                foreach (var ixc in illegalXmlValues)
                    tmpHeader[i] = tmpHeader[i].Replace(ixc, string.Empty);
                if (Regex.IsMatch(tmpHeader[i], @"^\d"))
                    tmpHeader[i] = '_'+ tmpHeader[i];
            }
            headers = tmpHeader;
            return true;
        }


        private static IEnumerable<string> ToCamelCase(IEnumerable<object> rowCells)
        {
            var sb = new StringBuilder();
            var totalCells = rowCells.Count();
            var result = new string[totalCells];
            for (var i = 0; i < totalCells; i++)
            {
                var curCell = rowCells.ElementAt(i);
                var words = curCell.ToString().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                sb.Append(char.ToLower(words[0][0]) + words[0].Substring(1).ToLower());

                for (var j = 1; j < words.Length; j++)
                    sb.Append(char.ToUpper(words[j][0]) + words[j].Substring(1).ToLower());

                result[i] = sb.ToString();
                sb.Clear();
            }
            return result;
        }
    }
}