using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetApplicationByReferenceApiRequest(Guid candidateId, string vacancyId) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}/applications/GetByReference/{vacancyId}";
    }
}
