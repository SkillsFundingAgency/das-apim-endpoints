using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;

public class GetCandidateSkillsQueryHandler(
    IRecruitApiClient<RecruitApiV2Configuration> apiClient,
    ICacheStorageService cacheStorageService)
    : IRequestHandler<GetCandidateSkillsQuery, GetCandidateSkillsQueryResponse>
{
    private const int CacheDurationInHours = 3;
    private const string CacheKey = "GetCandidateSkills";

    public async Task<GetCandidateSkillsQueryResponse> Handle(GetCandidateSkillsQuery request, CancellationToken cancellationToken)
    {
        var cachedSkills = await cacheStorageService.RetrieveFromCache<List<string>>(CacheKey);
        if (cachedSkills != null)
        {
            return new GetCandidateSkillsQueryResponse
            {
                CandidateSkills = cachedSkills
            };
        }
            
        var response = await apiClient.Get<List<string>>(new GetCandidateSkillsRequest());
        await cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
        return new GetCandidateSkillsQueryResponse()
        {
            CandidateSkills = response
        };
    }
}