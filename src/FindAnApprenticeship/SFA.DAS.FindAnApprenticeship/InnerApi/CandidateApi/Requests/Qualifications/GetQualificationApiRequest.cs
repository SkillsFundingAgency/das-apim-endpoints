using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.Qualifications
{
    public class GetQualificationApiRequest(Guid applicationId, Guid candidateId, Guid id) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}/applications/{applicationId}/qualifications/{id}";
    }
}
