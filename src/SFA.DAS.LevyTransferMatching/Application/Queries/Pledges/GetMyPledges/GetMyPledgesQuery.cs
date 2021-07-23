using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetMyPledges
{
    public class GetMyPledgesQuery : IRequest<GetMyPledgesQueryResult>
    {
        public long AccountId { get; set; }

        public GetMyPledgesQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}
