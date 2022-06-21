using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests
{
    public class GetCandidateSkillsRequest : IGetApiRequest
    {
        public string GetUrl => "api/referencedata/candidate-skills";
    }
}