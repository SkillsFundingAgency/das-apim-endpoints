using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

public class CreateApplicationReviewRequest(Guid applicationReviewId, CreateApplicationReviewRequestData data) : IPutApiRequest
{
    public string PutUrl => $"api/applicationReviews/{applicationReviewId}";
    public object Data { get; set; } = data;
}

public record CreateApplicationReviewRequestData(
    long AccountId,
    long AccountLegalEntityId,
    Guid ApplicationId,
    Guid CandidateId,
    int Ukprn,
    long VacancyReference,
    string VacancyTitle,
    string? AdditionalQuestion1,
    string? AdditionalQuestion2
)
{
    public ApplicationReviewStatus Status { get; } = ApplicationReviewStatus.New; // required
}