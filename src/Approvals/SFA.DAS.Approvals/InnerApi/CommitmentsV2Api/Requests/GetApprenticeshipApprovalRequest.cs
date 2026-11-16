using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class GetApprenticeshipApprovalRequest : IGetApiRequest
{
    public readonly long ApprenticeshipId;

    public GetApprenticeshipApprovalRequest(long apprenticeshipId)
    {
        ApprenticeshipId = apprenticeshipId;
    }

    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/approvals";
}
