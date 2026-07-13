using SFA.DAS.Apim.Shared.Interfaces;
namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class DeleteLearningRequest(Guid learnerKey) : IDeleteApiRequest
{
    public Guid LearnerKey { get; set; } = learnerKey;
    public string DeleteUrl => $"learning/{LearnerKey}";
}
