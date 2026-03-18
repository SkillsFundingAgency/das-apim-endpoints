using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;

public class GetCandidateQualificationsRequest: IGetApiRequest
{
    public string GetUrl => "api/referencedata/candidate-qualifications";
}