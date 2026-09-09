using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetApplicationReviewsByVacancyIdApiRequest(Guid VacancyId) : IGetApiRequest
    {
        public string GetUrl => $"api/vacancies/byId/{VacancyId}/applicationReviews";
    }
}