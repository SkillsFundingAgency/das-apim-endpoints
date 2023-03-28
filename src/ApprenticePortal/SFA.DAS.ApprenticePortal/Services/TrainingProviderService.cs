﻿using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Requests;
using SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Responses;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ApprenticePortal.Services
{
    public class TrainingProviderService
    {
        private readonly ProviderAccountApiClient _client;

        public TrainingProviderService(ProviderAccountApiClient client) => _client = client;

        public async Task<TrainingProviderResponse> GetTrainingProviderDetails(long trainingProviderId)
        {
            var searchResponse = await _client.GetWithResponseCode<SearchResponse>(new GetTrainingProviderDetailsRequest(trainingProviderId));

            if (searchResponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestContentException(searchResponse.ErrorContent, searchResponse.StatusCode);

            if (searchResponse.Body.SearchResults.Length == 0)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {trainingProviderId} not found",
                    System.Net.HttpStatusCode.NotFound,
                    "");
            }

            if (searchResponse.Body.SearchResults.Length > 1)
            {
                throw new HttpRequestContentException(
                    $"Training Provider Id {trainingProviderId} finds multiple matches",
                    System.Net.HttpStatusCode.Conflict,
                    "");
            }

            return searchResponse.Body.SearchResults.First();
        }
    }
}