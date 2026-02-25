using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class UpdateLearnerCommand : IRequest
{
    public Guid LearningKey { get; set; }
    public long Ukprn { get; set; }
    public UpdateLearnerRequest UpdateLearnerRequest { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public static class UpdateLearnerCommandExtensions
{
    public static List<KeyValuePair<string, List<LearningSupportRequestDetails>>> EnglishAndMathsLearningSupport(this UpdateLearnerCommand command)
    {
        if (command.UpdateLearnerRequest?.Delivery?.EnglishAndMaths == null)
        {
            return new List<KeyValuePair<string, List<LearningSupportRequestDetails>>>();
        }

        return command.UpdateLearnerRequest.Delivery.EnglishAndMaths
            .Select(x => new KeyValuePair<string, List<LearningSupportRequestDetails>>(x.LearnAimRef, x.LearningSupport)).ToList();
    }
}