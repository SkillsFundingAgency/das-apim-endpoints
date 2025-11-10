using MediatR;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Application.UpdateLearner;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class UpdateLearnerCommand : IRequest
{
    public Guid LearningKey { get; set; }
    public UpdateLearnerRequest UpdateLearnerRequest { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal static class UpdateLearnerCommandExtensions
{
    /// <summary>
    /// Retrieves a combined list of learning support details from both on-programme and maths and English courses within the UpdateLearnerRequest.
    /// </summary>
    internal static List<LearningSupportUpdatedDetails> CombinedLearningSupport(this UpdateLearnerCommand command)
    {
        var combinedLearningSupport = new List<LearningSupportUpdatedDetails>();

        combinedLearningSupport.AddRange(GetOnProgrammeLearningSupport(command));
        combinedLearningSupport.AddRange(GetEnglishAndMathsLearningSupport(command));

        return combinedLearningSupport;
    }

    private static IEnumerable<LearningSupportUpdatedDetails> GetOnProgrammeLearningSupport(UpdateLearnerCommand command)
    {
        var results = new List<LearningSupportUpdatedDetails>();
        var onProgramme = command.UpdateLearnerRequest.Delivery.OnProgramme;
        var completionDate = onProgramme.First().CompletionDate;
        var withdrawalDate = onProgramme.First().WithdrawalDate;

        foreach (var ls in onProgramme.First().LearningSupport)
        {
            var potentialEndDates = new List<DateTime> { ls.EndDate };

            if (completionDate.HasValue)
                potentialEndDates.Add(completionDate.Value);

            if (withdrawalDate.HasValue)
                potentialEndDates.Add(withdrawalDate.Value);

            var endDate = potentialEndDates.Min();

            results.Add(new LearningSupportUpdatedDetails
            {
                StartDate = ls.StartDate,
                EndDate = endDate
            });
        }

        return results;
    }

    private static IEnumerable<LearningSupportUpdatedDetails> GetEnglishAndMathsLearningSupport(UpdateLearnerCommand command)
    {
        var results = new List<LearningSupportUpdatedDetails>();
        var englishAndMaths = command.UpdateLearnerRequest.Delivery.EnglishAndMaths;

        foreach (var em in englishAndMaths)
        {
            foreach (var ls in em.LearningSupport)
            {
                var potentialEndDates = new List<DateTime> { ls.EndDate };

                if (em.CompletionDate.HasValue)
                    potentialEndDates.Add(em.CompletionDate.Value);

                if (em.WithdrawalDate.HasValue)
                    potentialEndDates.Add(em.WithdrawalDate.Value);

                var endDate = potentialEndDates.Min();

                results.Add(new LearningSupportUpdatedDetails
                {
                    StartDate = ls.StartDate,
                    EndDate = endDate
                });
            }
        }
    
        return results;
    }
}
