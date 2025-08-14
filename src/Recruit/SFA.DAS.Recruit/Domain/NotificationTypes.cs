using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationTypes
{
    VacancyApprovedOrRejectedByDfE,
    VacancyClosingSoon,
    ApplicationSubmitted,
    VacancySentForReview,
    VacancyRejectedByEmployer,
}