using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills
{
    public class GetCandidateSkillsQueryHandler : IRequestHandler<GetCandidateSkillsQuery, GetCandidateSkillsQueryResponse>
    {
        private const int CacheDurationInHours = 3;
        private const string CacheKey = "GetCandidateSkills";
        private readonly IRecruitApiClient<RecruitApiConfiguration> _apiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetCandidateSkillsQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
        }
        
        public async Task<GetCandidateSkillsQueryResponse> Handle(GetCandidateSkillsQuery request, CancellationToken cancellationToken)
        {
            var cachedSkills = await _cacheStorageService.RetrieveFromCache<List<string>>(CacheKey);

            if (cachedSkills != null)
            {
                return new GetCandidateSkillsQueryResponse
                {
                    CandidateSkills = cachedSkills
                };
            }
            
            var response =
                await _apiClient.Get<List<string>>(new GetCandidateSkillsRequest());

            await _cacheStorageService.SaveToCache(CacheKey, response, CacheDurationInHours);
            
            return new GetCandidateSkillsQueryResponse()
            {
                CandidateSkills = response
            };
        }
    }
}