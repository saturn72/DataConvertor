using System.IO;

namespace DataCovertor
{
    public interface IConvertor<out TOutput>
    {
        DatasourceType ConvertsFrom {get;}
        TOutput Convert(Stream stream);
    }
}