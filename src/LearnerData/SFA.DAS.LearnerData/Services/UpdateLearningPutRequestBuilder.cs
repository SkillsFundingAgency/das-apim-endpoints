using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateLearningPutRequestBuilder
{
    Task<UpdateLearningApiPutRequest> Build(long ukprn, UpdateLearnerRequest updateLearnerRequest, Guid learnerKey);
}

public class UpdateLearningPutRequestBuilder(IUpdateLearningRequestBodyBuilder requestBodyBuilder) : IUpdateLearningPutRequestBuilder
{
    public async Task<UpdateLearningApiPutRequest> Build(long ukprn, UpdateLearnerRequest updateLearnerRequest, Guid learnerKey)
    {
        var body = await requestBodyBuilder.Build(ukprn, updateLearnerRequest);
        return new UpdateLearningApiPutRequest(ukprn, learnerKey, body);
    }
}