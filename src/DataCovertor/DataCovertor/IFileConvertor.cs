namespace DataCovertor
{
    public interface IFileConvertor<out TOutput, TSettings> : IConvertor<TOutput, TSettings>
        where TSettings : IConvertorSettings
    {
        TOutput Convert(string filePath, TSettings settings);
    }
}