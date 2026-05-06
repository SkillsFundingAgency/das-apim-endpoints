using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
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