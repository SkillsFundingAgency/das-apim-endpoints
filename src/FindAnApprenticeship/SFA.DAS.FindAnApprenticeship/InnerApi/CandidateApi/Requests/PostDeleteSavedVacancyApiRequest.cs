using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record PostDeleteSavedVacancyApiRequest(Guid CandidateId, string VacancyId, bool DeleteAllByVacancyReference) : IDeleteApiRequest
    {
        public string DeleteUrl => $"api/candidates/{CandidateId}/saved-vacancies/{VacancyId}?deleteAllByReference={DeleteAllByVacancyReference}";
    }
}