using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests
{
    public class PostGetClosedVacanciesByReferenceApiRequest(PostGetClosedVacanciesByReferenceApiRequestBody body) : IPostApiRequest
    {
        public object Data { get; set; } = body;

        public string PostUrl => "api/vacancies/closed";
    }

    public class PostGetClosedVacanciesByReferenceApiRequestBody
    {
        public List<long> VacancyReferences { get; set; }
    }
}
