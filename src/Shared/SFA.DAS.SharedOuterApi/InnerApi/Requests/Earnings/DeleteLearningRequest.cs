using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;

public class DeleteLearningRequest(Guid learningKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; set; } = learningKey;
    public string DeleteUrl => $"learning/{LearningKey}";
}
