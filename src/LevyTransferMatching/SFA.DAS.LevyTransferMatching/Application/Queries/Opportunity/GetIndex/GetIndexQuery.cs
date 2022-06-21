using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public IEnumerable<string> Sectors { get; set; }
    }
}
