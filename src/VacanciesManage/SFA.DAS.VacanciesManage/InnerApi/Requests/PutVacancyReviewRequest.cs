using SFA.DAS.SharedOuterApi.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests;

public record PutVacancyReviewRequest(Guid Id, PutVacancyReviewRequest.PutVacancyReviewRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/vacancyReviews/{Id}";
    public object Data { get; set; } = Payload;

    public sealed record PutVacancyReviewRequestData
    {
        [Required]
        public required long VacancyReference { get; init; }
        public required string VacancyTitle { get; init; }
        public required DateTime? CreatedDate { get; init; }
        public required DateTime? SlaDeadLine { get; init; }
        public DateTime? ReviewedDate { get; init; }
        public required ReviewStatus Status { get; init; }
        public required byte? SubmissionCount { get; init; }
        public string? ReviewedByUserEmail { get; init; }
        public string? SubmittedByUserEmail { get; set; }
        public DateTime? ClosedDate { get; init; }
        public string? ManualOutcome { get; init; }
        public string? ManualQaComment { get; init; }
        public List<string>? ManualQaFieldIndicators { get; init; } = [];
        public string? AutomatedQaOutcome { get; init; }
        public string? AutomatedQaOutcomeIndicators { get; init; }
        public List<string>? DismissedAutomatedQaOutcomeIndicators { get; init; } = [];
        public List<string>? UpdatedFieldIdentifiers { get; init; } = [];
        public required string VacancySnapshot { get; init; }
        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public OwnerType OwnerType { get; set; }
        public string? SubmittedByUserId { get; set; }
    }
}