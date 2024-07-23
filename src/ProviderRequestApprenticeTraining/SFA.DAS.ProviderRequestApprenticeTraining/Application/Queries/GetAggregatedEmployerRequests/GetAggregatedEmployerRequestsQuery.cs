using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQuery : IRequest<GetAggregatedEmployerRequestsResult>
    {
        public long Ukprn { get; set; }

        public GetAggregatedEmployerRequestsQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
