#region Usings

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Shouldly;
using ToJsonCovertors.Excel;

#endregion

namespace ToJsonCovertors.Tests
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
        [TestCaseSource(nameof(GetAllEmptyFiles))]
        public void ExcelConvertor_ToExcel_EmptyFile(string file)
        {
            var ec = new ExcelConvertor();
            Should.Throw<ArgumentException>(() => ec.ToJson(file, null));
        }

        private static string[] GetAllEmptyFiles()
        {
            var resourcesPath = Path.Combine(GetCurrentAssemblyFolder(), "Resources");
            var files = Directory.GetFiles(resourcesPath, "empty*.*");
            return files;
        }

        [Test]
        [TestCaseSource(nameof(GetAllMissingFiles))]
        public void ExcelConvertor_ToExcel_MissingFile(string file)
        {
            var ec = new ExcelConvertor();
            var settings = new ExcelConvertorSettings
            {
                MandatoryColumns = new[] {"id", "wholesale", "UPC Code 1"}
            };
            Should.Throw<ArgumentException>(() => ec.ToJson(file, settings));
        }

        private static string[] GetAllMissingFiles()
        {
            var resourcesPath = Path.Combine(GetCurrentAssemblyFolder(), "Resources");
            var files = Directory.GetFiles(resourcesPath, "missing*.*");
            return files;
        }

        [Test]
        [TestCaseSource(nameof(GetAllGoodFiles))]
        public void ExcelConvertor_ToExcel_ConvertsPasses(string file)
        {
            const string expected = @"[{""id"":""1"",""sku"":""SKU_2"",""upcCode1"":""348132100244"",""description"":""Description_2"",""description[2]"":null,""productLine"":""NV"",""wholesale"":""7.9"",""12MonthAvg"":""902.333333333333"",""someDate"":""12/12/2017 12:00:00 AM"",""someTime"":""12/31/1899 9:12:00 AM"",""someDecimal"":""12.12"",""someInt"":""12"",""someFloat"":""12.33""},{""id"":""2"",""sku"":""SKU_3"",""upcCode1"":""896909001916"",""description"":""Description_3"",""description[2]"":null,""productLine"":""NET"",""wholesale"":""36"",""12MonthAvg"":""832.833333333333"",""someDate"":""1/1/2014 12:00:00 AM"",""someTime"":""12/31/1899 9:32:00 PM"",""someDecimal"":""11"",""someInt"":""3"",""someFloat"":""23.44""}]";
            new ExcelConvertor().ToJson(file, null).ShouldBe(expected);
        }

        private static string[] GetAllGoodFiles()
        {
            var resourcesPath = Path.Combine(GetCurrentAssemblyFolder(), "Resources");
            var files = Directory.GetFiles(resourcesPath, "good*.*");
            return files;
        }

        public static string GetCurrentAssemblyFolder()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }
    }
}