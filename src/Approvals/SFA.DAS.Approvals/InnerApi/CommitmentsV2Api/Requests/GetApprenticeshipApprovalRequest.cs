using SFA.DAS.Approvals.Enums;
using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class GetApprenticeshipApprovalRequest : IGetApiRequest
{
    public readonly long ApprenticeshipId;
    public readonly Guid ApprovalRequestId; 

    public GetApprenticeshipApprovalRequest(long apprenticeshipId, Guid approvalRequestId)
    {
        ApprenticeshipId = apprenticeshipId;
        ApprovalRequestId = approvalRequestId;
    }

    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/approvals/{ApprovalRequestId}";
}
