using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateLearningPutRequestBuilder
{
    UpdateLearningApiPutRequest Build(long ukprn, UpdateLearnerRequest updateLearnerRequest, Guid learningKey);
}

public class UpdateLearningPutRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : IUpdateLearningPutRequestBuilder
{
    public UpdateLearningApiPutRequest Build(long ukprn, UpdateLearnerRequest updateLearnerRequest, Guid learningKey)
    {
        var body = requestBodyBuilder.Build(ukprn, updateLearnerRequest);
        return new UpdateLearningApiPutRequest(learningKey, body);
    }
}