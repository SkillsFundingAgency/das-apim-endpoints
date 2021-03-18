using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi;
using System.Linq;
using System.Net;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class TrainingProviderService
    {
        private readonly IInternalApiClient<TrainingProviderConfiguration> _client;

        public TrainingProviderService(IInternalApiClient<TrainingProviderConfiguration> client) => _client = client;

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetPingRequest());
                return status == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TrainingProviderResponse> GetTrainingProviderDetails(long trainingProviderId)
        {
            var searchResponse = await _client.Get<SearchResponse>(new GetTrainingProviderDetailsRequest(trainingProviderId));

            if (searchResponse?.SearchResults == null || searchResponse.SearchResults.Length == 0)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {trainingProviderId} not found",
                    System.Net.HttpStatusCode.NotFound,
                    "");
            }

            if (searchResponse.SearchResults.Length > 1)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {trainingProviderId} finds multiple matches",
                    System.Net.HttpStatusCode.Conflict,
                    "");
            }
            
            return searchResponse.SearchResults.First();
        }
    }
}