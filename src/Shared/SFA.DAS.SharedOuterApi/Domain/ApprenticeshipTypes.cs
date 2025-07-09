using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprenticeshipTypes
{
    Standard,
    Foundation,
}