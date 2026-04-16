using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest(
    long VacancyReference,
    Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"api/applicationReviews/{VacancyReference}/candidate/{CandidateId}";
}