using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateEarningsLearningSupportRequestBuilder
{
    UpdateLearningSupportApiPutRequest Build(UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest);
}

public class UpdateEarningsLearningSupportRequestBuilder : IUpdateEarningsLearningSupportRequestBuilder
{
    public UpdateLearningSupportApiPutRequest Build(UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest)
    {
        var payload = new UpdateLearningSupportRequest
        {
            LearningSupport = putRequest.Data.LearningSupport.Select(x => new LearningSupport
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList()
        };

        return new UpdateLearningSupportApiPutRequest(learningApiPutResponse.LearningKey, payload);
    }
}