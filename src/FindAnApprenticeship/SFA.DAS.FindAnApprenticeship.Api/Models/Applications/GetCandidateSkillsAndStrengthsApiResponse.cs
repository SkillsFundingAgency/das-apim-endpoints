using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetCandidateSkillsAndStrengthsApiResponse
    {
        public string Strengths { get; set; }

        public static implicit operator GetCandidateSkillsAndStrengthsApiResponse(
            GetCandidateSkillsAndStrengthsQueryResult source)
        {
            return new GetCandidateSkillsAndStrengthsApiResponse
            {
                Strengths = source.Strengths
            };
        }
    }
}
