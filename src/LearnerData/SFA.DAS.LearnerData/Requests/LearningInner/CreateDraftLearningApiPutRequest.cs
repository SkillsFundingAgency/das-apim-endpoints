using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class CreateDraftLearningApiPutRequest : IPutApiRequest<UpdateLearningRequestBody>
{
    public string PutUrl { get; }

    public UpdateLearningRequestBody Data { get; set; }
    public long Ukprn { get; set; }

    public CreateDraftLearningApiPutRequest(UpdateLearningRequestBody data, long ukprn, long uln)
    {
        PutUrl = $"{ukprn}/apprenticeship{uln}";
        Data = data;
        Ukprn = ukprn;
    }
}