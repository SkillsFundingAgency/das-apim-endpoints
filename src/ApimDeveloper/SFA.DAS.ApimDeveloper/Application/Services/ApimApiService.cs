using System.Threading.Tasks;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.Services
{
    public class ApimApiService : IApimApiService
    {
        private const int CachedProductExpiryTimeInHours = 1;
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public ApimApiService (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient, ICacheStorageService cacheStorageService)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetAvailableApiProductsResponse> GetAvailableProducts(string accountType)
        {
            var cachedProducts = await _cacheStorageService.RetrieveFromCache<GetAvailableApiProductsResponse>($"{accountType}-{nameof(GetAvailableApiProductsResponse)}");

            if (cachedProducts != null)
            {
                return cachedProducts;
            }
            
            var products =
                await _apimDeveloperApiClient.Get<GetAvailableApiProductsResponse>(
                    new GetAvailableApiProductsRequest(accountType));

            await _cacheStorageService.SaveToCache($"{accountType}-{nameof(GetAvailableApiProductsResponse)}", products,
                CachedProductExpiryTimeInHours);
            
            return products;
        }
    }
}