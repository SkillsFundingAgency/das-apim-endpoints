
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;
    
public class DeleteLearningRequest(Guid learningKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; set; } = learningKey;
    public string DeleteUrl => $"learning/{LearningKey}";
}
