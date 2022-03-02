using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Forecasting.Application.Courses.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
    }

    public class GetPledgesQueryResult
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public long AccountId { get; set; }
        }
    }

    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        public Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
