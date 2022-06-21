using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.VacanciesManage.Configuration;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.Interfaces;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications
{
    public class GetQualificationsQueryHandler : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResponse>
    {
        private const int CacheDurationInHours = 3;
        private const string CacheKey = "GetQualifications";
        private readonly IRecruitApiClient<RecruitApiConfiguration> _apiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetQualificationsQueryHandler (IRecruitApiClient<RecruitApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetQualificationsQueryResponse> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            var cacheResponse = await _cacheStorageService.RetrieveFromCache<List<string>>(CacheKey);

            if (cacheResponse != null)
            {
                return new GetQualificationsQueryResponse
                {
                    Qualifications = cacheResponse
                };
            }
            
            var response =
                await _apiClient.Get<List<string>>(new GetQualificationsRequest());

            await _cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
            
            return new GetQualificationsQueryResponse
            {
                Qualifications = response
            };
        }
    }
}