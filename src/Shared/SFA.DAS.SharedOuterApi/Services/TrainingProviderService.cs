using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.TrainingProviderService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IInternalApiClient<TrainingProviderConfiguration> _client;

        public TrainingProviderService(IInternalApiClient<TrainingProviderConfiguration> client) => _client = client;

        public async Task<TrainingProviderResponse> GetTrainingProviderDetails(long ukprn)
        {
            var searchResponse = await _client.GetWithResponseCode<SearchResponse>(new GetTrainingProviderDetailsRequest((int)ukprn));

            if (searchResponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestContentException(searchResponse.ErrorContent, searchResponse.StatusCode);

            if (searchResponse.Body.SearchResults.Length == 0)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {ukprn} not found",
                    System.Net.HttpStatusCode.NotFound,
                    "");
            }

            if (searchResponse.Body.SearchResults.Length > 1)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {ukprn} finds multiple matches",
                    System.Net.HttpStatusCode.Conflict,
                    "");
            }

            return searchResponse.Body.SearchResults.First();
        }
    }
}