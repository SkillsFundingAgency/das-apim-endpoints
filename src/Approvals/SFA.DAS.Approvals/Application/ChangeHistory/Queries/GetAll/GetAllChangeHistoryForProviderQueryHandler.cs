using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForProviderQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        : IRequestHandler<GetAllChangeHistoryForProviderQuery, GetAllChangeHistoryForProviderQueryResult>
{
    public async Task<GetAllChangeHistoryForProviderQueryResult> Handle(GetAllChangeHistoryForProviderQuery query, CancellationToken cancellationToken)
    {
        var changeHistory = await commitmentsV2ApiClient.Get<GetAllChangeHistoryForProviderResponse>(new GetAllChangeHistoryForProviderRequest(query.ProviderId));

        var allChangeHistory = changeHistory?.ChangeHistory?.ToList() ?? [];

        var offset = Math.Min((long)(query.PageNumber - 1) * query.PageSize, allChangeHistory.Count);

        var page = allChangeHistory
            .Skip((int)offset)
            .Take(query.PageSize)
            .ToList();

        return new GetAllChangeHistoryForProviderQueryResult
        {
            ChangeHistory = page,
            TotalCount = allChangeHistory.Count
        };
    }
}