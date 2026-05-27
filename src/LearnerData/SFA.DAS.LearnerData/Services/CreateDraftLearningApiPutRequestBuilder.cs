using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateDraftLearningApiPutRequestBuilder
{
    CreateDraftLearningApiPutRequest Build(long ukprn, UpdateLearnerRequest updateLearnerRequest);
}

public class CreateDraftLearningApiPutRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : ICreateDraftLearningApiPutRequestBuilder
{
    public CreateDraftLearningApiPutRequest Build(long ukprn, UpdateLearnerRequest updateLearnerRequest)
    {
        var body = requestBodyBuilder.Build(ukprn, updateLearnerRequest);
        return new CreateDraftLearningApiPutRequest(body, ukprn, updateLearnerRequest.Learner.Uln);
    }
}
