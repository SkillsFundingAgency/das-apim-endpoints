using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetWorkHistoriesApiRequest : IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;

        public GetWorkHistoriesApiRequest(Guid applicationId, Guid candidateId)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
        }

        public string GetUrl => $"candidates/{_candidateId}/applications/{_applicationId}/work-history";
        public object Data { get; set; }        
    }
}
