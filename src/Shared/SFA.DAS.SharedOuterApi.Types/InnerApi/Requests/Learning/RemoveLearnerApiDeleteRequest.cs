using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

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