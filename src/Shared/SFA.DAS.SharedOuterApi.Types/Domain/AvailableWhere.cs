using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvailableWhere
{
    OneLocation,
    MultipleLocations,
    AcrossEngland,
}