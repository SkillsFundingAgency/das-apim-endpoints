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

        var onProgrammeCompletionDate = command.UpdateLearnerRequest.Delivery.OnProgramme.CompletionDate;
        var onProgrammeLearningSupport = command.UpdateLearnerRequest.Delivery.OnProgramme?.LearningSupport
            .Select(ls => new LearningSupportUpdatedDetails
            {
                StartDate = ls.StartDate,
                EndDate = onProgrammeCompletionDate.HasValue && onProgrammeCompletionDate.Value < ls.EndDate ? onProgrammeCompletionDate.Value : ls.EndDate
            });

        if (onProgrammeLearningSupport != null && onProgrammeLearningSupport.Any())
            combinedLearningSupport.AddRange(onProgrammeLearningSupport);

        var mathsAndEnglishLearningSupport = command.UpdateLearnerRequest.Delivery.EnglishAndMaths?
            .SelectMany(x => x.LearningSupport != null
                ? x.LearningSupport.Select(ls => new LearningSupportUpdatedDetails
                {
                    StartDate = ls.StartDate,
                    EndDate = x.CompletionDate.HasValue && x.CompletionDate.Value < ls.EndDate ? x.CompletionDate.Value : ls.EndDate
                })
                : Enumerable.Empty<LearningSupportUpdatedDetails>());


        if (mathsAndEnglishLearningSupport != null && mathsAndEnglishLearningSupport.Any())
            combinedLearningSupport.AddRange(mathsAndEnglishLearningSupport);


        return combinedLearningSupport;
    }
}
