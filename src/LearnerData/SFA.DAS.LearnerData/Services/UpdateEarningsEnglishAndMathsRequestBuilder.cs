using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateEarningsEnglishAndMathsRequestBuilder
{
    UpdateEnglishAndMathsApiPutRequest Build(UpdateLearnerCommand command, UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest);
}

public class UpdateEarningsEnglishAndMathsRequestBuilder : IUpdateEarningsEnglishAndMathsRequestBuilder
{
    public UpdateEnglishAndMathsApiPutRequest Build(UpdateLearnerCommand command,
        UpdateLearnerApiPutResponse learningApiPutResponse, UpdateLearningApiPutRequest putRequest)
    {
        var body = new UpdateEnglishAndMathsRequest
        {
            EnglishAndMaths = command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Select(x => new EnglishAndMathsItem
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                LearnAimRef = x.LearnAimRef,
                Course = x.Course,
                Amount = x.Amount,
                WithdrawalDate = x.WithdrawalDate,
                PriorLearningAdjustmentPercentage = x.PriorLearningPercentage,
                ActualEndDate = x.CompletionDate ?? x.ActualEndDate,
                PauseDate = x.PauseDate
            }).ToList()
        };
            
        return new UpdateEnglishAndMathsApiPutRequest(command.LearningKey, body);
    }
}