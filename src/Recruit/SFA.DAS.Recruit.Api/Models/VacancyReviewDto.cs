using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models;

public class VacancyReviewDto
{
    public Guid Id { get; init; }
    public long VacancyReference { get; init; }
    public required string VacancyTitle { get; init; }
    public required DateTime CreatedDate { get; init; }
    public required DateTime SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public required string Status { get; init; }
    public byte SubmissionCount { get; init; }
    public string ReviewedByUserEmail { get; init; }
    public required string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string ManualOutcome { get; set; }
    public string ManualQaComment { get; init; }
    public required List<string> ManualQaFieldIndicators { get; init; }
    public string AutomatedQaOutcome { get; set; }
    public string AutomatedQaOutcomeIndicators { get; init; }
    public required List<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public required List<string> UpdatedFieldIdentifiers { get; init; }
    public required string VacancySnapshot { get; set; }
    public long AccountLegalEntityId { get; set; }

    public string OwnerType { get; set; }

    public long AccountId { get; set; }

    public long Ukprn { get; set; }

    public static explicit operator VacancyReviewDto(GetVacancyReviewResponse source)
    {
        return new VacancyReviewDto
        {
            Id = source.Id,
            VacancyReference = source.VacancyReference,
            VacancyTitle =  source.VacancyTitle,
            CreatedDate = source.CreatedDate,
            SlaDeadLine = source.SlaDeadLine,
            ReviewedDate = source.ReviewedDate,
            Status = source.Status,
            SubmissionCount = source.SubmissionCount,
            ReviewedByUserEmail = source.ReviewedByUserEmail,
            SubmittedByUserEmail = source.SubmittedByUserEmail,
            ClosedDate = source.ClosedDate,
            ManualOutcome = source.ManualOutcome,
            ManualQaComment = source.ManualQaComment,
            ManualQaFieldIndicators = source.ManualQaFieldIndicators.ToList(),
            AutomatedQaOutcome = source.AutomatedQaOutcome,
            AutomatedQaOutcomeIndicators = source.AutomatedQaOutcomeIndicators,
            DismissedAutomatedQaOutcomeIndicators = source.DismissedAutomatedQaOutcomeIndicators,
            UpdatedFieldIdentifiers = source.UpdatedFieldIdentifiers,
            VacancySnapshot = source.VacancySnapshot,
            OwnerType = source.OwnerType
        };
    }

    public static explicit operator InnerApi.Requests.VacancyReviewDto(VacancyReviewDto source)
    {
        return new InnerApi.Requests.VacancyReviewDto
        {
            VacancyReference = source.VacancyReference,
            VacancyTitle =  source.VacancyTitle,
            CreatedDate = source.CreatedDate,
            SlaDeadLine = source.SlaDeadLine,
            ReviewedDate = source.ReviewedDate,
            Status = source.Status,
            SubmissionCount = source.SubmissionCount,
            ReviewedByUserEmail = source.ReviewedByUserEmail,
            SubmittedByUserEmail = source.SubmittedByUserEmail,
            ClosedDate = source.ClosedDate,
            ManualOutcome = source.ManualOutcome,
            ManualQaComment = source.ManualQaComment,
            ManualQaFieldIndicators = source.ManualQaFieldIndicators.ToList(),
            AutomatedQaOutcome = source.AutomatedQaOutcome,
            AutomatedQaOutcomeIndicators = source.AutomatedQaOutcomeIndicators,
            DismissedAutomatedQaOutcomeIndicators = source.DismissedAutomatedQaOutcomeIndicators,
            UpdatedFieldIdentifiers = source.UpdatedFieldIdentifiers,
            VacancySnapshot = source.VacancySnapshot,
            Ukprn = source.Ukprn,
            AccountId = source.AccountId,
            OwnerType = source.OwnerType,
            AccountLegalEntityId = source.AccountLegalEntityId
        };
    }
}