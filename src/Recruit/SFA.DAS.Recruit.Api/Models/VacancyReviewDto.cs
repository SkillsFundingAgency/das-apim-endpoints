using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Models;
using OwnerType = SFA.DAS.Recruit.Domain.Vacancy.OwnerType;

namespace SFA.DAS.Recruit.Api.Models;

public class VacancyReviewDto
{
    public Guid Id { get; init; }
    public long VacancyReference { get; init; }
    public required string VacancyTitle { get; init; }
    public required DateTime CreatedDate { get; init; }
    public required DateTime SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public required ReviewStatus Status { get; init; }
    public byte SubmissionCount { get; init; }
    public string ReviewedByUserEmail { get; init; }
    public string? SubmittedByUserEmail { get; init; }
    public string SubmittedByUserId { get; init; }
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
    public OwnerType OwnerType { get; set; }
    public long AccountId { get; set; }
    public int Ukprn { get; set; }
    public string HashedAccountId { get; set; }
    public string EmployerName { get; set; }
    public Guid VacancyId { get; set; }
    public List<Address> EmployerLocations { get; set; }
    public AvailableWhere EmployerLocationOption { get; set; }

    public static explicit operator VacancyReviewDto(GetVacancyReviewResponse source)
    {
        return new VacancyReviewDto
        {
            Id = source.Id,
            VacancyReference = ParseVacancyReference(source.VacancyReference),
            VacancyTitle =  source.VacancyTitle,
            CreatedDate = source.CreatedDate,
            SlaDeadLine = source.SlaDeadLine,
            ReviewedDate = source.ReviewedDate,
            Status = source.Status,
            SubmissionCount = source.SubmissionCount,
            ReviewedByUserEmail = source.ReviewedByUserEmail,
            SubmittedByUserEmail = source.SubmittedByUserEmail,
            SubmittedByUserId = source.SubmittedByUserId.ToString(),
            ClosedDate = source.ClosedDate,
            ManualOutcome = source.ManualOutcome,
            ManualQaComment = source.ManualQaComment,
            ManualQaFieldIndicators = source.ManualQaFieldIndicators.ToList(),
            AutomatedQaOutcome = source.AutomatedQaOutcome,
            AutomatedQaOutcomeIndicators = source.AutomatedQaOutcomeIndicators,
            DismissedAutomatedQaOutcomeIndicators = source.DismissedAutomatedQaOutcomeIndicators,
            UpdatedFieldIdentifiers = source.UpdatedFieldIdentifiers,
            VacancySnapshot = source.VacancySnapshot,
            OwnerType = source.OwnerType,
            VacancyId = source.Id
        };
    }

    

    public static explicit operator InnerApi.Recruit.Requests.VacancyReviewDto(VacancyReviewDto source)
    {
        return new InnerApi.Recruit.Requests.VacancyReviewDto
        {
            VacancyReference = source.VacancyReference.ToString(),
            VacancyTitle =  source.VacancyTitle,
            CreatedDate = source.CreatedDate,
            SlaDeadLine = source.SlaDeadLine,
            ReviewedDate = source.ReviewedDate,
            Status = source.Status,
            SubmissionCount = source.SubmissionCount,
            ReviewedByUserEmail = source.ReviewedByUserEmail,
            SubmittedByUserEmail = source.SubmittedByUserEmail,
            SubmittedByUserId = source.SubmittedByUserId,
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
            AccountLegalEntityId = source.AccountLegalEntityId,
            EmployerName = source.EmployerName,
            HashedAccountId = source.HashedAccountId,
            EmployerLocations = source.EmployerLocations,
            EmployerLocationOption = source.EmployerLocationOption,
            VacancyId = source.VacancyId,
        };
    }

    private static long ParseVacancyReference(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
            return 0;

        reference = reference.ToUpperInvariant();

        return reference.StartsWith("VAC") &&
               long.TryParse(reference.AsSpan(3), out var result)
            ? result
            : 0;
    }
}