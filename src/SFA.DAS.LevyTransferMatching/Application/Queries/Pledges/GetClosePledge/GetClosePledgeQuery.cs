using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetClosePledge
{
    public class GetClosePledgeQuery : IRequest<GetClosePledgeQueryResult>
    {
        public int PledgeId { get; set; }
    }

}
