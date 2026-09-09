using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries;

public class GetChangeHistoryQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        : IRequestHandler<GetChangeHistoryQuery, GetChangeHistoryResult>
{
    public async Task<GetChangeHistoryResult> Handle(GetChangeHistoryQuery query, CancellationToken cancellationToken)
    {
        var changeHistory = await commitmentsV2ApiClient.Get<GetChangeHistoryResponse>(new GetChangeHistoryRequest(query.ApprenticeshipId));

        return new GetChangeHistoryResult
        {
            ChangeHistory = changeHistory.ChangeHistory
        };
    }
}