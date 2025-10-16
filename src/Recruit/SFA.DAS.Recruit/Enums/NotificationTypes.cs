using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums;

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
