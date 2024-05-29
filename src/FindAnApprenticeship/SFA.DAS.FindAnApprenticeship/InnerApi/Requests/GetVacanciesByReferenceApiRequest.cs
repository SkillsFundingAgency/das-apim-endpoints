using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests
{
    public class PostGetVacanciesByReferenceApiRequest(PostGetVacanciesByReferenceApiRequestBody body) : IPostApiRequest
    {
        public object Data { get; set; } = body;

        public string PostUrl => $"/api/vacancies";

        public string Version => "2.0";
    }

    public class PostGetVacanciesByReferenceApiRequestBody
    {
        public List<string> VacancyReferences { get; set; }
    }

}
