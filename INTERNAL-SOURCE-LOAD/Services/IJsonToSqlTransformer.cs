using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD;

public interface IJsonToSqlTransformer<T>
{
    string Transform(T data);
}
