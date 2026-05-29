using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateDraftLearningApiPostRequestBuilder
{
    CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest);
}

public class CreateDraftLearningApiPostRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : ICreateDraftLearningApiPostRequestBuilder
{
    public CreateDraftLearningApiPostRequest Build(long ukprn, CreateLearnerRequest createLearnerRequest)
    {
        var body = requestBodyBuilder.Build(ukprn, createLearnerRequest);
        return new CreateDraftLearningApiPostRequest(body, ukprn, createLearnerRequest.Learner.Uln);
    }
}
