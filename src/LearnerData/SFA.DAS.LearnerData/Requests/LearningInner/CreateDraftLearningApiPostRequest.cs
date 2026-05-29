using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class CreateDraftLearningApiPostRequest : IPostApiRequest
{
    public string PostUrl { get; }

    public object Data { get; set; }
    public long Ukprn { get; set; }

    public CreateDraftLearningApiPostRequest(UpdateLearningRequestBody data, long ukprn, long uln)
    {
        PostUrl = $"{ukprn}/apprenticeships/{uln}";
        Data = data;
        Ukprn = ukprn;
    }
}
