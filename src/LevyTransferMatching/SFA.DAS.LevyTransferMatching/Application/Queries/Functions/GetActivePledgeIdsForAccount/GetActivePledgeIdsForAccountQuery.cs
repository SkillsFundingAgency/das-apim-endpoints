using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;

public class GetActivePledgeIdsForAccountQuery : IRequest<GetActivePledgeIdsForAccountQueryResult>
{
    public long AccountId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}
