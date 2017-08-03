#region Usings

using System.IO;

#endregion

namespace DataCovertor
{
    public interface IConvertor<out TOutput, TSettings> where TSettings : IConvertorSettings
    {
        DatasourceType ConvertsFrom { get; }
        TOutput Convert(Stream stream, TSettings settings);
    }
}