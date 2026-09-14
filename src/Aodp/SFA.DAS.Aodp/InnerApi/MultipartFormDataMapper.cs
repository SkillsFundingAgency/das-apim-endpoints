using System.Text.Json;

namespace SFA.DAS.Aodp.InnerApi;

public static class MultipartFormDataMapper
{
    public static IEnumerable<KeyValuePair<string, string>> Map(object data)
    {
        var element = JsonSerializer.SerializeToElement(data);
        return Flatten(element, string.Empty);
    }

    private static IEnumerable<KeyValuePair<string, string>> Flatten(JsonElement element, string key)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    var propertyKey = string.IsNullOrEmpty(key)
                        ? property.Name
                        : $"{key}.{property.Name}";

                    foreach (var value in Flatten(property.Value, propertyKey))
                    {
                        yield return value;
                    }
                }

                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in element.EnumerateArray())
                {
                    var itemKey = item.ValueKind is JsonValueKind.Object or JsonValueKind.Array
                        ? $"{key}[{index}]"
                        : key;

                    foreach (var value in Flatten(item, itemKey))
                    {
                        yield return value;
                    }

                    index++;
                }

                break;

            case JsonValueKind.String:
                yield return new KeyValuePair<string, string>(key, element.GetString()!);
                break;

            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                yield return new KeyValuePair<string, string>(key, element.GetRawText());
                break;
        }
    }
}
