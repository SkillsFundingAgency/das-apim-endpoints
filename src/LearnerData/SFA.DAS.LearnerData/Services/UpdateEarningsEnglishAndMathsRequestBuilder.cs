using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Helpers;
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
            EnglishAndMaths = putRequest.Data.MathsAndEnglishCourses.Select(x => new EnglishAndMathsItem
            {
                StartDate = x.StartDate,
                EndDate = x.PlannedEndDate,
                LearnAimRef = x.LearnAimRef,
                Course = x.Course,
                Amount = x.Amount,
                WithdrawalDate = x.WithdrawalDate,
                PriorLearningAdjustmentPercentage = x.PriorLearningPercentage,
                ActualEndDate = x.CompletionDate ?? x.PauseDate ?? x.WithdrawalDate,
                PauseDate = x.PauseDate,
                PeriodsInLearning = GetPeriodsInLearning(x.LearnAimRef, command)
            }).ToList()
        };
            
        return new UpdateEnglishAndMathsApiPutRequest(command.LearningKey, body);
    }

    private List<PeriodInLearningItem> GetPeriodsInLearning(string learnAimRef, UpdateLearnerCommand command)
    {
        var periodsInLearning = new List<PeriodInLearningItem>();

        var matchingEnglishAndMaths =
            command.UpdateLearnerRequest.Delivery.EnglishAndMaths.Where(x => x.LearnAimRef == learnAimRef);

        //my thinking here is that we need to iterate the learning api's put request, because that's where the E&M elements have been "grouped"
        //but no, even easier is to just get the matching elements because OnProg aligns nicely with E&M elements
        foreach (var mathsAndEnglish in matchingEnglishAndMaths)
        {
            var endDate = DateTimeHelper.EarliestOf(mathsAndEnglish.EndDate,
                mathsAndEnglish.CompletionDate,
                mathsAndEnglish.PauseDate,
                mathsAndEnglish.WithdrawalDate,
                mathsAndEnglish.CompletionDate) ?? mathsAndEnglish.EndDate;

            periodsInLearning.Add(new PeriodInLearningItem
            {
                StartDate = mathsAndEnglish.StartDate,
                EndDate = endDate,
                OriginalExpectedEndDate = mathsAndEnglish.EndDate
            });
        }

        return periodsInLearning.OrderBy(x => x.StartDate).ToList();
    }
}