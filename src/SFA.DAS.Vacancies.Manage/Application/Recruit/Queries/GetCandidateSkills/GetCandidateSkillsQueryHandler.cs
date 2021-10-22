using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills
{
    public class GetCandidateSkillsQueryHandler : IRequestHandler<GetCandidateSkillsQuery, GetCandidateSkillsQueryResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _apiClient;

        public GetCandidateSkillsQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<GetCandidateSkillsQueryResponse> Handle(GetCandidateSkillsQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<List<string>>(new GetCandidateSkillsRequest());

            return new GetCandidateSkillsQueryResponse()
            {
                CandidateSkills = response
            };
        }
    }
}