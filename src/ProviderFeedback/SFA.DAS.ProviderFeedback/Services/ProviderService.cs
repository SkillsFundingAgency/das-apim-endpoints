using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Services
{
    public interface IProviderService
    {
        Task<TrainingProviderResponse> GetTrainingProviderDetails(long providerId);
    }

    public class ProviderService : IProviderService
    {
        private readonly ITrainingProviderService _trainingProviderService;
        private readonly ICacheStorageService _cacheStorageService;

        public const string ProviderDetailsCacheKey = "ProviderService.TrainingProviderResponse";
        public const int CacheExpiryHours = 12;

        public ProviderService(
            ITrainingProviderService trainingProviderService,
            ICacheStorageService cacheStorageService)
        {
            _trainingProviderService = trainingProviderService;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<TrainingProviderResponse> GetTrainingProviderDetails(long providerId)
        {
            var cacheKey = $"{ProviderDetailsCacheKey}-{providerId}";

            var cacheResult = await _cacheStorageService.RetrieveFromCache<TrainingProviderResponse>(cacheKey);

            if (cacheResult != null)
            {
                return cacheResult;
            }

            var result = await _trainingProviderService.GetTrainingProviderDetails(providerId);
            await _cacheStorageService.SaveToCache(cacheKey, result, CacheExpiryHours);
            return result;
        }
    }
}
