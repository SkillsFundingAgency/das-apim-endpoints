using MediatR;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Infrastructure;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex
{
    public class GetIndexQuery : PagedQuery, IRequest<GetIndexQueryResult>
    {
        public IEnumerable<string> Sectors { get; set; }
    }
}
