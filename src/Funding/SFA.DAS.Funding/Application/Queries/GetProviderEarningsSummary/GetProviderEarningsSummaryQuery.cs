using MediatR;

namespace SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary
{
    public class GetProviderEarningsSummaryQuery : IRequest<GetProviderEarningsSummaryResult>
    {
        public long Ukprn { get ; set ; }
    }
}