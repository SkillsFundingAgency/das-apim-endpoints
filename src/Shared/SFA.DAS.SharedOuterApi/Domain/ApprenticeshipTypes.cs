using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain;

[JsonConverter(typeof(ApprenticeshipTypeConverter))]
public enum ApprenticeshipTypes
{
    Standard,
    Foundation,
}

public class ApprenticeshipTypeConverter : JsonConverter<ApprenticeshipTypes>
{
    public override ApprenticeshipTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return ApprenticeshipTypes.Standard;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected token type {reader.TokenType}. Expected String or Null.");
        }

        string stringValue = reader.GetString();

        string normalizedValue = stringValue?.ToLowerInvariant();

        switch (normalizedValue)
        {
            case "apprenticeship":
            case "apprenticeshipstandard":
                return ApprenticeshipTypes.Standard;

            case "foundation":
            case "foundationapprenticeship":
                return ApprenticeshipTypes.Foundation;

            default:
                throw new JsonException($"Unable to convert \"{stringValue}\" to ApprenticeshipTypes.");
        }
    }

    public override void Write(Utf8JsonWriter writer, ApprenticeshipTypes value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ApprenticeshipTypes.Standard:
                writer.WriteStringValue("Standard");
                break;
            case ApprenticeshipTypes.Foundation:
                writer.WriteStringValue("Foundation");
                break;
            default:
                writer.WriteStringValue(value.ToString());
                break;
        }
    }
}