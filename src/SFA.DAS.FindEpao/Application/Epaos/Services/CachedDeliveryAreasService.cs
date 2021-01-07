using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Epaos.Services
{
    public class CachedDeliveryAreasService : ICachedDeliveryAreasService
    {
        private const int DeliveryAreaCacheDurationInHours = 1;

        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public CachedDeliveryAreasService(
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ICacheStorageService cacheStorageService)
        {
            _assessorsApiClient = assessorsApiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<IReadOnlyList<GetDeliveryAreaListItem>> GetDeliveryAreas()
        {
            var deliveryAreas = await _cacheStorageService
                .RetrieveFromCache<IReadOnlyList<GetDeliveryAreaListItem>>(nameof(GetDeliveryAreasRequest));
            
            if (deliveryAreas != null) 
                return deliveryAreas;
            
            deliveryAreas = (await _assessorsApiClient.GetAll<GetDeliveryAreaListItem>(new GetDeliveryAreasRequest())).ToList();
            await _cacheStorageService.SaveToCache(nameof(GetDeliveryAreasRequest), deliveryAreas, DeliveryAreaCacheDurationInHours);

            return deliveryAreas;
        }
    }
}