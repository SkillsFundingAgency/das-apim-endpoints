using System;
using SFA.DAS.ApprenticeCommitments.Types;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SFA.DAS.ApprenticeCommitments.Extensions
{
    public static class Extensions
    {
        public static string ToIsoDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
        
        public static int GetApprenticeshipType(this string value, int defaultValue = 0)
        {
            if (Enum.TryParse(typeof(ApprenticeshipType), value, true, out var result))
            {
                return (int)result;
            }
            return defaultValue;
        }        
    }

    public class StringFromNumberConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64().ToString();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            throw new JsonException($"Unexpected token parsing string. Token: {reader.TokenType}");        
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
