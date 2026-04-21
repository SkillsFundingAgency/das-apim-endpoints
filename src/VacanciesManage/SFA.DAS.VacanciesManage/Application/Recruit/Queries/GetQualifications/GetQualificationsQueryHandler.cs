using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;

public class GetQualificationsQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
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

        var response = await recruitApiClient.Get<List<string>>(new GetReferencedataCandidateQualificationsApiRequest());
        await cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
        return new GetQualificationsQueryResponse
        {
            Qualifications = response
        };
    }
}