namespace DataCovertor
{
    public interface IFileConvertor<in TSettings> : IConvertor<TSettings>
        where TSettings : IConvertorSettings
    {
        string ToJson(string filePath, TSettings settings);
    }
}