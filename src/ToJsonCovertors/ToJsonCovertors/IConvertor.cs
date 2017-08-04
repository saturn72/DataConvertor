#region Usings

using System.IO;

#endregion

namespace ToJsonCovertors
{
    public interface IConvertor<in TSettings> where TSettings : IConvertorSettings
    {
        DatasourceType ConvertsFrom { get; }
        string ToJson(Stream stream, TSettings settings);
    }
}