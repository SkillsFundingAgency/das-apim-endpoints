using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class GetCandidateSkillsRequest: IGetApiRequest
{
    public string GetUrl => "api/referencedata/candidate-skills";
}