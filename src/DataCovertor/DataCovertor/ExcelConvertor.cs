using System;
using System.IO;
using System.Xml.Linq;
using ExcelDataReader;

namespace DataCovertor
{
    public class ExcelConvertor : IFileConvertor<XDocument>
    {
        public DatasourceType ConvertsFrom => DatasourceType.Excel;

        public XDocument Convert(Stream stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var hf = reader.HeaderFooter;
                reader.Read();
                throw new NotImplementedException();
            }
        }

        public XDocument Convert(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                return Convert(stream);
            }
        }
    }
}