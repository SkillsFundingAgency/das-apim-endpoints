using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;
        private readonly ICacheStorageService _cacheStorageService;
        private const string CacheKeyPrefix = "ReferenceData.";
        private const int CacheExpiryInHours = 2;

        public ReferenceDataService(ICacheStorageService cacheStorageService, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _cacheStorageService = cacheStorageService;
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<List<ReferenceDataItem>> GetLevels()
        {
            return await GetFromApiWithCache(new GetLevelsRequest());
        }

        public async Task<List<ReferenceDataItem>> GetSectors()
        {
            return await GetFromApiWithCache(new GetSectorsRequest());
        }

        public async Task<List<ReferenceDataItem>> GetJobRoles()
        {
            return await GetFromApiWithCache(new GetJobRolesRequest());
        }

        private async Task<List<ReferenceDataItem>> GetFromApiWithCache(IGetAllApiRequest request)
        {
            var result = await _cacheStorageService.RetrieveFromCache<List<ReferenceDataItem>>($"{CacheKeyPrefix}{request.GetAllUrl}");
            if (result != null) return result;

            var response = (await _levyTransferMatchingApiClient.GetAll<ReferenceDataItem>(request)).ToList();
            await _cacheStorageService.SaveToCache($"{CacheKeyPrefix}{request.GetAllUrl}", response, CacheExpiryInHours);
            return response;
        }
    }
}
