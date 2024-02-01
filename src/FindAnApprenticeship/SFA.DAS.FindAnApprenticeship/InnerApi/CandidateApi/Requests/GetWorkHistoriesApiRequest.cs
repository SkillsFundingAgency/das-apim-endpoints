using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetWorkHistoriesApiRequest : IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly WorkHistoryType _workHistoryType;

        public GetWorkHistoriesApiRequest(Guid applicationId, Guid candidateId, WorkHistoryType workHistoryType)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _workHistoryType = workHistoryType;
        }

        public string GetUrl =>
            $"candidates/{_candidateId}/applications/{_applicationId}/work-history?workHistoryType={_workHistoryType}";

        public object Data { get; set; }        
    }
}
