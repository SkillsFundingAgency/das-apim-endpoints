using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetPendingStartDateChangeRequest : IGetApiRequest
{
    public GetPendingStartDateChangeRequest(Guid learningKey)
    {
        LearningKey = learningKey;
    }

    public Guid LearningKey { get; }
    public string GetUrl => $"{LearningKey}/startDateChange/pending";
}