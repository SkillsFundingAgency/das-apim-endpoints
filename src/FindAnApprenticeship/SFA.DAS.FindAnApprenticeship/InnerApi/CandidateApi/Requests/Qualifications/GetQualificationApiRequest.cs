using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.Qualifications
{
    internal class GetQualificationApiRequest(Guid applicationId, Guid candidateId, Guid id) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}/applications/{applicationId}/qualifications/{id}";
    }
}
