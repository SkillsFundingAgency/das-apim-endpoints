using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using static SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference.GetApprenticeshipVacancyQueryResult;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record GetSavedVacancyApiRequest(Guid CandidateId, string VacancyId, string VacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{CandidateId}/saved-vacancies/{VacancyReference}?vacancyId={VacancyId}";
    }
}