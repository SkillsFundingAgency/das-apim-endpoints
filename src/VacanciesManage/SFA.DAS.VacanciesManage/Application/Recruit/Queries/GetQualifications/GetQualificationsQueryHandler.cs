using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;

public class GetQualificationsQueryHandler(
    IRecruitApiClient<RecruitApiV2Configuration> apiClient,
    ICacheStorageService cacheStorageService)
    : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResponse>
{
    private const int CacheDurationInHours = 3;
    private const string CacheKey = "GetQualifications";

    public async Task<GetQualificationsQueryResponse> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
    {
        var cacheResponse = await cacheStorageService.RetrieveFromCache<List<string>>(CacheKey);
        if (cacheResponse != null)
        {
            return new GetQualificationsQueryResponse
            {
                Qualifications = cacheResponse
            };
        }
            
        var response = await apiClient.Get<List<string>>(new GetCandidateQualificationsRequest());
        await cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
        return new GetQualificationsQueryResponse
        {
            Qualifications = response
        };
    }
}