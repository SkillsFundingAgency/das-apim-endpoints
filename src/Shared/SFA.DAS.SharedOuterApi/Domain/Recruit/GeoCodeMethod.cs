using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GeoCodeMethod
{
    Unspecified,
    ExistingVacancy,
    PostcodesIo,
    Loqate,
    PostcodesIoOutcode,
    OuterApi,
    FailedToGeoCode
}