using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class RemoveLearnerApiDeleteRequest : IDeleteApiRequest
{
    public RemoveLearnerApiDeleteRequest(Guid learnerKey, long ukprn)
    {
        LearnerKey = learnerKey;
        Ukprn = ukprn;
    }

    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
    public string DeleteUrl => $"{Ukprn}/{LearnerKey}";
}