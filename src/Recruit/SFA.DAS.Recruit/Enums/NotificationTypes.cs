using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationTypes
{
    VacancyApprovedOrRejected,
    VacancyClosingSoon,
    ApplicationSubmitted,
    VacancySentForReview,
    SharedApplicationReviewedByEmployer,
    ProviderAttachedToVacancy,
    ApplicationSharedWithEmployer,
}
