using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetLearnerStatusRequest : IGetApiRequest
{
    public GetLearnerStatusRequest(Guid learningKey)
    {
        LearningKey = learningKey;
    }

    public Guid LearningKey { get; }
    public string GetUrl => $"{LearningKey}/LearnerStatus";
}