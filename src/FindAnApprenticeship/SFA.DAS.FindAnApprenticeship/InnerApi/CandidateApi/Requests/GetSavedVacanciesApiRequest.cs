using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetSavedVacanciesApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/{candidateId}/saved-vacancies";
}