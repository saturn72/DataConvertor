namespace DataCovertor
{
    public interface IFileConvertor<out TOutput>:IConvertor<TOutput>
    {
        TOutput Convert(string filePath);
    }
}