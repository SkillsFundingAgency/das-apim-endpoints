using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class GetCandidateQualificationsRequest: IGetApiRequest
{
    public string GetUrl => "api/referencedata/candidate-qualifications";
}