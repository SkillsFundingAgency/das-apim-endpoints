using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class PutCreateVacancyReviewRequest(Guid id, VacancyReviewDto data) : IPutApiRequest
{
    public string PutUrl => $"api/vacancyreviews/{id}";
    public object Data { get; set; } = data;
}

public class VacancyReviewDto
{
    public required long VacancyReference { get; init; }
    public required string VacancyTitle { get; init; }
    public required DateTime? CreatedDate { get; init; }
    public required DateTime? SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public required string Status { get; init; }
    public required byte? SubmissionCount { get; init; }
    public string? ReviewedByUserEmail { get; init; }
    public required string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string? ManualOutcome { get; init; }
    public string? ManualQaComment { get; init; }
    public required List<string> ManualQaFieldIndicators { get; init; }
    public string? AutomatedQaOutcome { get; init; }
    public string? AutomatedQaOutcomeIndicators { get; init; }
    public required List<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public required List<string> UpdatedFieldIdentifiers { get; init; }
    public required string VacancySnapshot { get; init; }
    public long Ukprn { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string OwnerType { get; set; }
}