using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateEarningsLearningSupportRequestBuilder
{
    UpdateLearningSupportApiPutRequest Build(UpdateLearnerCommand command, UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest);
}

public class UpdateEarningsLearningSupportRequestBuilder : IUpdateEarningsLearningSupportRequestBuilder
{
    public UpdateLearningSupportApiPutRequest Build(UpdateLearnerCommand command, UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest)
    {
        var payload = new UpdateLearningSupportRequest
        {
            LearningSupport = putRequest.Data.LearningSupport.Select(x => new LearningSupportItem
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList()
        };

        return new UpdateLearningSupportApiPutRequest(command.LearningKey, payload);
    }
}