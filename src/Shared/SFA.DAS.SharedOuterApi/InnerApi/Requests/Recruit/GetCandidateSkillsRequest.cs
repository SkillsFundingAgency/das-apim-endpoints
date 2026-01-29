using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;

public class GetCandidateSkillsRequest: IGetApiRequest
{
    public string GetUrl => "api/referencedata/candidate-skills";
}