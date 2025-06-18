using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetLearnerStatusRequest : IGetApiRequest
{
    public GetLearnerStatusRequest(Guid learningKey)
    {
        LearningKey = learningKey;
    }

    public Guid LearningKey { get; }
    public string GetUrl => $"{LearningKey}/LearnerStatus";
}