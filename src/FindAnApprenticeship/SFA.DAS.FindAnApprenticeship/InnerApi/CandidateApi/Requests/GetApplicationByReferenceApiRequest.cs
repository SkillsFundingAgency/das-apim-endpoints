using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetApplicationByReferenceApiRequest(Guid candidateId, string vacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}/applications/GetByReference/{vacancyReference}";
    }
}
