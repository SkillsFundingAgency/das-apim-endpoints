using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.InnerApi.Requests;
using SFA.DAS.Funding.Interfaces;

namespace SFA.DAS.Funding.Application.Services
{
    public class FundingApprenticeshipEarningsService : IFundingApprenticeshipEarningsService
    {
        private readonly IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration> _client;

        public FundingApprenticeshipEarningsService(IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetHealthRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
    }
}