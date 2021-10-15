using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public long AccountId { get; set; }

        public GetPledgesQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}
