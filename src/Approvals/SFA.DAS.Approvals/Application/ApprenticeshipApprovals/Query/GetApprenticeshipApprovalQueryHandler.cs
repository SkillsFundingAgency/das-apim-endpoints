using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;

public class GetApprenticeshipApprovalQueryHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient) : IRequestHandler<GetApprenticeshipApprovalQuery, GetApprenticeshipApprovalResponse?>
{
    public async Task<GetApprenticeshipApprovalResponse?> Handle(GetApprenticeshipApprovalQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetApprenticeshipApprovalResponse>(new GetApprenticeshipApprovalRequest(request.ApprenticeshipId, request.ApprovalRequestId));

        if (result == null) 
            return null;

        if(result.AccountId != request.EmployerAccountId)
            throw new UnauthorizedAccessException("This Employer does not have access to this apprenticeship approval.");

        return result;
    }
}