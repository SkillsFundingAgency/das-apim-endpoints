using System.Text.Json;
using System.Text.Json.Serialization;
using SFA.DAS.Common.Domain.Models;

namespace SFA.DAS.SharedOuterApi.Types.Json;

public class JsonVacancyReferenceConverter: JsonConverter<VacancyReference?>
{
    public override VacancyReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new VacancyReference(reader.GetString()),
            JsonTokenType.Number => new VacancyReference(reader.GetInt64()),
            _ => VacancyReference.None
        };
    }

    public override void Write(Utf8JsonWriter writer, VacancyReference? value, JsonSerializerOptions options)
    {
        if (value is null || value == VacancyReference.None)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.ToString());    
        }
    }
}