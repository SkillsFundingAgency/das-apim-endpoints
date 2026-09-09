using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;

public class GetActivePledgeIdsForAccountQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
    : IRequestHandler<GetActivePledgeIdsForAccountQuery, GetActivePledgeIdsForAccountQueryResult>
{
    public async Task<GetActivePledgeIdsForAccountQueryResult> Handle(GetActivePledgeIdsForAccountQuery request,
        CancellationToken cancellationToken)
    {
        var pledgesResponse = await levyTransferMatchingService.GetPledges(
            new GetPledgesRequest(
                accountId: request.AccountId,
                pledgeStatusFilter: PledgeStatus.Active,
                page: request.Page,
                pageSize: request.PageSize));

        return new GetActivePledgeIdsForAccountQueryResult
        {
            PledgeIds = pledgesResponse?.Pledges?.Select(p => p.Id).ToArray() ?? [],
            Page = pledgesResponse?.Page ?? request.Page,
            TotalPages = pledgesResponse?.TotalPages ?? 0,
            TotalPledges = pledgesResponse?.TotalPledges ?? 0
        };
    }
}
