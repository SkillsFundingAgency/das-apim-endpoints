using MediatR;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
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
