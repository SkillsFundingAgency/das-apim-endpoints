using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetCandidateApiRequest(Guid candidateId) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}";
    }
}
