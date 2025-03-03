using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetWorkHistoryItemApiRequest : IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly WorkHistoryType _workHistoryType;
        private readonly Guid _workHistoryItemId;

        public GetWorkHistoryItemApiRequest(Guid applicationId, Guid candidateId, Guid workHistoryItemId, WorkHistoryType workHistoryType)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _workHistoryType = workHistoryType;
            _workHistoryItemId = workHistoryItemId;
        }
        public string GetUrl => $"api/candidates/{_candidateId}/applications/{_applicationId}/work-history/{_workHistoryItemId}?workHistoryType={_workHistoryType}";
    }
}
