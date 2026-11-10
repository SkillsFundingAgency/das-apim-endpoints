using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprovalRequests;

public class GetApprovalRequestQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        : IRequestHandler<GetApprovalRequestQuery, GetApprovalRequestQueryResult>
{
    public async Task<GetApprovalRequestQueryResult> Handle(GetApprovalRequestQuery query, CancellationToken cancellationToken)
    {
        var approvalRequestResponse = await commitmentsV2ApiClient.GetWithResponseCode<GetApprovalRequestResponse>(new GetApprovalRequest(query.ApprenticeshipId, query.Status, query.AccountId));

        if (approvalRequestResponse.StatusCode == System.Net.HttpStatusCode.NotFound) { return null; }

        return new GetApprovalRequestQueryResult
        {
            ApprenticeName = approvalRequestResponse.Body?.ApprenticeName,
            ApprovalRequests = approvalRequestResponse.Body?.ApprovalRequests ?? []
        };
    }
}