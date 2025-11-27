using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services;

public interface IBreaksInLearningService
{
    List<BreakInLearning> CalculateOnProgrammeBreaksInLearning(List<OnProgrammeRequestDetails> onProgrammeItems);
    List<BreakInLearning> CalculateEnglishAndMathsBreaksInLearning(List<MathsAndEnglish> englishAndMathsItems);
}

public class BreaksInLearningService : IBreaksInLearningService
{
    public List<BreakInLearning> CalculateOnProgrammeBreaksInLearning(List<OnProgrammeRequestDetails> onProgrammeItems)
    {
        var breaks = new List<BreakInLearning>();

        for (var i = 0; i < onProgrammeItems.Count; i++)
        {
            var current = onProgrammeItems[i];

            // Breaks in learning (skip last item check)
            if (i < onProgrammeItems.Count - 1)
            {
                var next = onProgrammeItems[i + 1];

                if (current.ActualEndDate.HasValue && current.ActualEndDate < next.StartDate)
                {
                    var gapStart = current.ActualEndDate.Value.AddDays(1);
                    var gapEnd = next.StartDate.AddDays(-1);

                    breaks.Add(new BreakInLearning
                    {
                        StartDate = gapStart,
                        EndDate = gapEnd
                    });
                }
            }
        }

        return breaks;
    }

    public List<BreakInLearning> CalculateEnglishAndMathsBreaksInLearning(List<MathsAndEnglish> englishAndMathsItems)
    {
        throw new NotImplementedException();
    }
}