using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

public class GetLearnerStatusRequest : IGetApiRequest
{
    public GetLearnerStatusRequest(Guid learningKey)
    {
        LearningKey = learningKey;
    }

    public Guid LearningKey { get; }
    public string GetUrl => $"{LearningKey}/LearnerStatus";
}