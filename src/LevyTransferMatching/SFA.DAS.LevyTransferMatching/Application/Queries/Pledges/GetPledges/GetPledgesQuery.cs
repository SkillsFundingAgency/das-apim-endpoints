using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public long AccountId { get; set; }
        public int Page { get; set; }
        public int? PageSize { get; set; }

        public GetPledgesQuery(long accountId, int page = 1, int? pageSize = null)
        {
            AccountId = accountId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
