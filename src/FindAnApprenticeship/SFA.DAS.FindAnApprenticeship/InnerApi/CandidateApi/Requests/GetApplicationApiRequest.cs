using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetApplicationApiRequest : IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;

        public GetApplicationApiRequest(Guid candidateId, Guid applicationId)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
        }

        public string GetUrl => $"api/candidates/{_candidateId}/applications/{_applicationId}";
    }
}
