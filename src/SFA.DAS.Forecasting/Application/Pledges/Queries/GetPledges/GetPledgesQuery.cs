using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges
{
    public class GetPledgesQuery : IRequest<GetPledgesQueryResult>
    {
        public int Page { get; set; } 
        public int PageSize { get; set; }
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
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetPledgesQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
