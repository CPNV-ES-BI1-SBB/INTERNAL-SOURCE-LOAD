using System.Text.Json;
using INTERNAL_SOURCE_LOAD.Models;

namespace INTERNAL_SOURCE_LOAD.Services;

public class JsonToSqlTransformer<T> : IJsonToSqlTransformer<T>
{
    private readonly Func<T, string> _generateSql;

    public JsonToSqlTransformer(Func<T, string> generateSql)
    {
        _generateSql = generateSql ?? throw new ArgumentNullException(nameof(generateSql));
    }

    public string Transform(T data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        return _generateSql(data);
    }
    public static T Deserialize<T>(JsonElement jsonData)
    {
        return JsonSerializer.Deserialize<T>(jsonData.GetRawText(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new ArgumentException($"Invalid JSON format for {typeof(T).Name}.");
    }
}