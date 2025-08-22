using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationScope
{
    NotSet,
    UserSubmittedVacancies,
    OrganisationVacancies,
}