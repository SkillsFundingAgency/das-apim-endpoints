using System.Net;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Services
{
    public class LevyTransferMatchingApiClient : ILevyTransferMatchingApiClient
    {
        private readonly IInternalApiClient<LevyTransferMatchingApiConfiguration> _client;

        public LevyTransferMatchingApiClient(IInternalApiClient<LevyTransferMatchingApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> IsHealthy()
        {
            var status = await _client.GetResponseCode(new GetPingRequest());
            return status == HttpStatusCode.OK;
        }
    }
}
