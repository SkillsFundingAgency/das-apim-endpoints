using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetProfanitiesRequest: IGetApiRequest
{
    public string GetUrl => "api/prohibitedcontent/Profanity";
}