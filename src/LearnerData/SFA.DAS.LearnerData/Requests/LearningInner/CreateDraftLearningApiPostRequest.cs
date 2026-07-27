using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class CreateDraftLearningApiPostRequest : IPostApiRequest
{
    public string PostUrl { get; }

    public object Data { get; set; }
    public long Ukprn { get; set; }

    public CreateDraftLearningApiPostRequest(UpdateLearningRequestBody data, long ukprn)
    {
        PostUrl = $"{ukprn}/apprenticeships";
        Data = data;
        Ukprn = ukprn;
    }
}
