using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary
{
    public class GetProviderEarningsSummaryHandler : IRequestHandler<GetProviderEarningsSummaryQuery, GetProviderEarningsSummaryResult>
    {
        private readonly IFundingProviderEarningsService _providerEarningsService;

        public GetProviderEarningsSummaryHandler(IFundingProviderEarningsService providerEarningsService)
        {
            _providerEarningsService = providerEarningsService;
        }

        public async Task<GetProviderEarningsSummaryResult> Handle(GetProviderEarningsSummaryQuery request, CancellationToken cancellationToken)
        {
            var summary = await _providerEarningsService.GetSummary(request.Ukprn);

            var summaryToReturn = MapSummary(summary);

            return new GetProviderEarningsSummaryResult
            {
                Summary = summaryToReturn
            };
        }

        private ProviderEarningsSummary MapSummary(ProviderEarningsSummaryDto summaryDto)
        {
            return new ProviderEarningsSummary
            {
                TotalEarningsForCurrentAcademicYear = summaryDto.TotalEarningsForCurrentAcademicYear
            };
        }
    }
}