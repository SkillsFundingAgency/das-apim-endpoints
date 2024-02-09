using System;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetDeleteJobApiRequest : IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly Guid _workHistoryItemId;
        private readonly WorkHistoryType _workHistoryType;

        public GetDeleteJobApiRequest(Guid applicationId, Guid candidateId, Guid workHistoryItemId, WorkHistoryType workHistoryType)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _workHistoryItemId = workHistoryItemId;
            _workHistoryType = workHistoryType;

        }
        public string GetUrl =>
            $"candidates/{_candidateId}/applications/{_applicationId}/work-history/{_workHistoryItemId}?workHistoryType={_workHistoryType}";
    }
}
