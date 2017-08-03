using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace DataCovertor.Tests
{
    [TestFixture]
    public class ExcelConvertorTests
    {
        [Test]
        public void ExcelConvertor_ConvertsForm_EqualExcel()
        {
            new ExcelConvertor().ConvertsFrom.ShouldBe(DatasourceType.Excel);
        }

        [Test]
        public void ExcelConvertor_ToExcel_Passes()
        {
            var resourcesPath = Path.Combine(GetCurrentAssemblyFolder(), "Resources");
            var files = Directory.GetFiles(resourcesPath, "good.*");

            var ec = new ExcelConvertor();
            foreach (var file in files)
            {
                ec.Convert(file);
            }
            throw new NotImplementedException();
        }

        public string GetCurrentAssemblyFolder()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }
    }
}
