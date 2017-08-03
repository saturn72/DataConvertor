#region Usings

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

#endregion

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
        [TestCaseSource(nameof(GetAllEmptyFiles))]
        public void ExcelConvertor_ToExcel_EmptyFile(string file)
        {
            var ec = new ExcelConvertor();
            Should.Throw<ArgumentException>(() => ec.Convert(file, null));
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
                XmlRootNodeName = "items",
                XmlIterativeNodeName = "item",
                MandatoryColumns = new[] {"id", "wholesale", "UPC Code 1"}
            };
            Should.Throw<ArgumentException>(() => ec.Convert(file, settings));
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
            var expected = @"<root>
  <item>
    <id>1</id>
    <sku>SKU_2</sku>
    <upcCode1>348132100244</upcCode1>
    <description>Description_2</description>
    <description2 />
    <productLine>NV</productLine>
    <wholesale>7.9</wholesale>
    <_12MonthAvg>902.333333333333</_12MonthAvg>
  </item>
  <item>
    <id>2</id>
    <sku>SKU_3</sku>
    <upcCode1>896909001916</upcCode1>
    <description>Description_3</description>
    <description2 />
    <productLine>NET</productLine>
    <wholesale>36</wholesale>
    <_12MonthAvg>832.833333333333</_12MonthAvg>
  </item>
  <item>
    <id>3</id>
    <sku>SKU_4</sku>
    <upcCode1>782421519100</upcCode1>
    <description>Description_4</description>
    <description2>PACK CD</description2>
    <productLine>BAT</productLine>
    <wholesale>1.25</wholesale>
    <_12MonthAvg>765.416666666667</_12MonthAvg>
  </item>
  <item>
    <id>4</id>
    <sku>SKU_5</sku>
    <upcCode1>9342851001494</upcCode1>
    <description>Description_5</description>
    <description2>FUNCTIONS</description2>
    <productLine>NV</productLine>
    <wholesale>30.5</wholesale>
    <_12MonthAvg>728.083333333333</_12MonthAvg>
  </item>
  <item>
    <id>5</id>
    <sku>SKU_6</sku>
    <upcCode1>9342851001661</upcCode1>
    <description>Description_6</description>
    <description2>FUNCTIONS</description2>
    <productLine>NV</productLine>
    <wholesale>30.5</wholesale>
    <_12MonthAvg>684.416666666667</_12MonthAvg>
  </item>
</root>";
            var ec = new ExcelConvertor();
            var res = ec.Convert(file, null);
            res.ToString().ShouldBe(expected);
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