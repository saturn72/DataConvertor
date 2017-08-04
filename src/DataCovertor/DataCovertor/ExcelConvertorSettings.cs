#region Usings

using System.Collections.Generic;

#endregion

namespace DataCovertor
{
    public class ExcelConvertorSettings : IConvertorSettings
    {
        private IEnumerable<string> _mandatoryColumns;

        public IEnumerable<string> MandatoryColumns
        {
            get => _mandatoryColumns ?? (_mandatoryColumns = new string[] { });
            set => _mandatoryColumns = value;
        }
    }
}