using System.Text.Json;

namespace INTERNAL_SOURCE_LOAD;

public interface IJsonToSqlTransformer
{
    bool CanHandle(JsonElement jsonData);
    string Transform(JsonElement jsonData);
}
