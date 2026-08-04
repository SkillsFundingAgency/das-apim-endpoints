using System.Text.Json.Serialization;
using SFA.DAS.Common.Domain.Models;
using SFA.DAS.SharedOuterApi.Types.Json;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;

public record VacancyReview
{
    public Guid Id { get; init; }
    [JsonConverter(typeof(JsonVacancyReferenceConverter))]
    public VacancyReference VacancyReference { get; init; }
    public string VacancyTitle { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public ReviewStatus Status { get; init; }
    public int SubmissionCount { get; init; }
    public string? ReviewedByUserEmail { get; init; }
    public string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public ManualQaOutcome? ManualOutcome { get; init; }
    public string? ManualQaComment { get; init; }
    public List<string> ManualQaFieldIndicators { get; init; }
    public string? AutomatedQaOutcome { get; init; }
    public string? AutomatedQaOutcomeIndicators { get; init; }
    public List<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public List<string> UpdatedFieldIdentifiers { get; init; }
    public string VacancySnapshot { get; init; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long Ukprn { get; set; }
    public OwnerType OwnerType { get; set; }
}