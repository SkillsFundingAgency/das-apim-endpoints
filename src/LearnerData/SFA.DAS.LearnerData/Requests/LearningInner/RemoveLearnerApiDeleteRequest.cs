using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class RemoveLearnerApiDeleteRequest : IDeleteApiRequest
{
    public RemoveLearnerApiDeleteRequest(Guid learningKey, long ukprn)
    {
        LearningKey = learningKey;
        Ukprn = ukprn;
    }

    public Guid LearningKey { get; set; }
    public long Ukprn { get; set; }
    public string DeleteUrl => $"{Ukprn}/{LearningKey}";
}
