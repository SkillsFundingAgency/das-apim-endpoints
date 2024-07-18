using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public class TotalPositionsAvailableService(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        ICacheStorageService cacheStorageService) : ITotalPositionsAvailableService
    {
        public async Task<long> GetTotalPositionsAvailable()
        {
            var cachedValue = await cacheStorageService.RetrieveFromCache<long?>(nameof(GetTotalPositionsAvailableRequest));

            if (cachedValue.HasValue)
            {
                return cachedValue.Value;
            }

            var totalPositionsAvailable = await recruitApiClient.Get<long>(new GetTotalPositionsAvailableRequest());

            await cacheStorageService.SaveToCache(nameof(GetTotalPositionsAvailableRequest), totalPositionsAvailable,
                    TimeSpan.FromHours(1));

            return totalPositionsAvailable;
        }
    }
}
