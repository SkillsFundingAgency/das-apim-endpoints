using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;

public class GetCandidateSkillsQueryHandler(SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration> recruitApiClient,
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

        var response = await recruitApiClient.Get<List<string>>(new GetReferencedataCandidateSkillsApiRequest());
        await cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
        return new GetCandidateSkillsQueryResponse()
        {
            CandidateSkills = response
        };
    }
}