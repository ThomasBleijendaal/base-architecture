using Microsoft.Extensions.Primitives;

namespace Common.Extensions;

public static class DictionaryExtensions
{
    public static TObject? ToObject<TObject>(this IEnumerable<KeyValuePair<string, StringValues>> dictionary, JsonSerializerOptions options)
        => ToObject<string, string, TObject>(dictionary.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())), options);

    public static TObject? ToObject<TKey, TValue, TObject>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, JsonSerializerOptions options)
        where TKey : notnull
    {
        var json = JsonSerializer.Serialize(dictionary.ToDictionary(x => x.Key, x => x.Value), options);
        return JsonSerializer.Deserialize<TObject>(json, options);
    }
}
