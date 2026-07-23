using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Json;

// need this as remote services are passing different values
public class JsonDisabilityConfidentBooleanConverter: JsonConverter<bool?>
{
    public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String when bool.TryParse(reader.GetString(), out var value) => value,
            JsonTokenType.Number when reader.TryGetInt32(out var intValue) => Convert.ToBoolean(intValue),
            JsonTokenType.True or JsonTokenType.False => reader.GetBoolean(),
            JsonTokenType.Null => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteBooleanValue(value!.Value);
        }
    }
}