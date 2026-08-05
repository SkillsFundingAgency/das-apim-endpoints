using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

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