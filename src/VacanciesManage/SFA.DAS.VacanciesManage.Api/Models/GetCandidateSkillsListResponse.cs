using System.Collections.Generic;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;

namespace SFA.DAS.VacanciesManage.Api.Models
{
    public class GetCandidateSkillsListResponse
    {
        public IList<string> CandidateSkills { get; set; }

        public static implicit operator GetCandidateSkillsListResponse(GetCandidateSkillsQueryResponse source)
        {
            return new GetCandidateSkillsListResponse
            {
                CandidateSkills = source?.CandidateSkills ?? new List<string>() 
            };
        }
    }
}