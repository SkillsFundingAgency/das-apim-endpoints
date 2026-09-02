using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ApprovalRequest.Queries;

public class GetApprovalRequestQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        : IRequestHandler<GetApprovalRequestQuery, GetApprovalRequestQueryResult>
{
    public async Task<GetApprovalRequestQueryResult> Handle(GetApprovalRequestQuery query, CancellationToken cancellationToken)
    {
        var approvalRequestResponse = await commitmentsV2ApiClient.Get<GetApprovalRequestResponse>(new GetApprovalRequest(query.ApprenticeshipId, query.Status));

        return new GetApprovalRequestQueryResult
        {
            ApprenticeName = approvalRequestResponse?.ApprenticeName,
            ApprovalRequests = approvalRequestResponse?.ApprovalRequests ?? []
        };
    }
}