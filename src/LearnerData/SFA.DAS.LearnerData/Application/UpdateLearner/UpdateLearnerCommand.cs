using MediatR;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class UpdateLearnerCommand : IRequest
{
    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
    public UpdateLearnerRequest UpdateLearnerRequest { get; set; }
}

public static class UpdateLearnerRequestExtensions
{
    public static List<KeyValuePair<string, List<LearningSupport>>> EnglishAndMathsLearningSupport(this UpdateLearnerRequest request)
    {
        if (request?.Delivery?.EnglishAndMaths == null)
        {
            return new List<KeyValuePair<string, List<LearningSupport>>>();
        }

        return request.Delivery.EnglishAndMaths
            .Select(x => new KeyValuePair<string, List<LearningSupport>>(x.LearnAimRef, x.LearningSupport)).ToList();
    }

    public static List<KeyValuePair<string, List<LearningSupport>>> EnglishAndMathsLearningSupport(this CreateLearnerRequest request)
    {
        if (request?.Delivery?.EnglishAndMaths == null)
        {
            return new List<KeyValuePair<string, List<LearningSupport>>>();
        }

        return request.Delivery.EnglishAndMaths
            .Select(x => new KeyValuePair<string, List<LearningSupport>>(x.LearnAimRef, x.LearningSupport)).ToList();
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.