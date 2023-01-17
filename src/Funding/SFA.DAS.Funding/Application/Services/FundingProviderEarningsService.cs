using System.Threading.Tasks;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.InnerApi.Requests.ProviderEarnings;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;

namespace SFA.DAS.Funding.Application.Services
{
    public class FundingProviderEarningsService : IFundingProviderEarningsService
    {
        private readonly IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration> _client;

        public FundingProviderEarningsService(IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration> client)
        {
            _client = client;
        }

        public async Task<ProviderEarningsSummaryDto> GetSummary(long ukprn)
        {
            var response = await _client.Get<ProviderEarningsSummaryDto>(new GetProviderEarningsSummaryRequest(ukprn));

            return response;
        }
    }
}