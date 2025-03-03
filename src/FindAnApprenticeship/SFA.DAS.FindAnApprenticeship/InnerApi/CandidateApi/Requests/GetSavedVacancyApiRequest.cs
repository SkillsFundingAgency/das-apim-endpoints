using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record GetSavedVacancyApiRequest(Guid CandidateId, string VacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{CandidateId}/saved-vacancies/{VacancyReference}";
    }
}