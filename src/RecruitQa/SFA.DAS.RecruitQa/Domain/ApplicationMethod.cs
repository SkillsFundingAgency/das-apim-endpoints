using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationMethod
{
    ThroughFindAnApprenticeship,
    ThroughExternalApplicationSite,
    ThroughFindATraineeship,
    Unspecified,
}