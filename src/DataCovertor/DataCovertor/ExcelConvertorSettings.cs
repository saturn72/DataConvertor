#region Usings

using System.Collections.Generic;

#endregion

namespace DataCovertor
{
    public class ExcelConvertorSettings : IConvertorSettings
    {
        private static ExcelConvertorSettings _default;

        public static ExcelConvertorSettings Default => _default ?? (_default = new
                                                            ExcelConvertorSettings
                                                            {
                                                                XmlRootNodeName = "root",
                                                                XmlIterativeNodeName = "item",
                                                                MandatoryColumns = new string[] { }
                                                            });

        public IEnumerable<string> MandatoryColumns { get; set; }

        public string XmlRootNodeName { get; set; }
        public string XmlIterativeNodeName { get; set; }
    }
}