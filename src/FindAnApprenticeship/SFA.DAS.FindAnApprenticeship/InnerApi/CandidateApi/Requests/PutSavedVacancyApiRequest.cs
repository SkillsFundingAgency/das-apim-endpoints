using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PutSavedVacancyApiRequest(Guid candidateId, PostSavedVacancyApiRequestData data)
    : IPutApiRequest
    {
        public object Data { get; set; } = data;

        public string PutUrl => $"api/candidates/{candidateId}/saved-vacancies";
    }
    public class PostSavedVacancyApiRequestData
    {
        public string VacancyId { get; set; }
        public string VacancyReference { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
