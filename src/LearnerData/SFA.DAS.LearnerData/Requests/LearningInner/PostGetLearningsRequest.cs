using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

/// <summary>
/// Gets learnings (with episodes and prices) for a list of learning keys.
/// </summary>
/// <remarks>Although this is a Post, no data is added or modified in the learning inner domain</remarks>
public class PostGetLearningsRequest : IPostApiRequest
{
    public long Ukprn { get; }

    public string PostUrl => $"{Ukprn}/learnings/by-keys";

    public object Data { get; set; }

    public PostGetLearningsRequest(long ukprn, List<Guid> learningKeys)
    {
        Ukprn = ukprn;
        Data = learningKeys;
    }
}
