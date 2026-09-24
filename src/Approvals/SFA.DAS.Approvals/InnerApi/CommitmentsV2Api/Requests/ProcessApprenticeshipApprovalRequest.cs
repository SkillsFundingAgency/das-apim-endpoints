using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class ProcessApprenticeshipApprovalRequest : IPostApiRequest
    {
        public long ApprenticeshipId { get; }
        public Guid ApprovalRequestId { get; }
        public string PostUrl => $"api/apprenticeships/{ApprenticeshipId}/approvals/{ApprovalRequestId}";
        public object Data { get; set; }

        public ProcessApprenticeshipApprovalRequest(long apprenticeshipId, Guid approvalRequestId, Body body)
        {
            ApprenticeshipId = apprenticeshipId;
            ApprovalRequestId = approvalRequestId;
            Data = body;
        }

        public class Body : SaveDataRequest
        {
            public bool ApplyChanges { get; set; }
        }
    }
}
