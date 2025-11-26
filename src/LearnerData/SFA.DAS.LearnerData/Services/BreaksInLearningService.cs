using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

namespace SFA.DAS.LearnerData.Services;

public interface IBreaksInLearningService
{
    (List<BreakInLearning> Breaks, List<CostDetails> Costs) CalculateOnProgrammeBreaksInLearning(List<OnProgrammeRequestDetails> onProgrammeItems);
    List<BreakInLearning> CalculateEnglishAndMathsBreaksInLearning(List<MathsAndEnglish> englishAndMathsItems);
}

public class BreaksInLearningService : IBreaksInLearningService
{
    public (List<BreakInLearning> Breaks, List<CostDetails> Costs) CalculateOnProgrammeBreaksInLearning(List<OnProgrammeRequestDetails> onProgrammeItems)
    {
        var breaks = new List<BreakInLearning>();
        var mergedCosts = new List<CostDetails>();

        for (var i = 0; i < onProgrammeItems.Count; i++)
        {
            var current = onProgrammeItems[i];

            // Merge costs: union all, but discard redundant consecutive entries
            if (current.Costs != null)
            {
                foreach (var cost in current.Costs)
                {
                    if (mergedCosts.Count == 0)
                    {
                        mergedCosts.Add(cost);
                    }
                    else
                    {
                        var last = mergedCosts.Last();
                        if (last.TrainingPrice == cost.TrainingPrice &&
                            last.EpaoPrice == cost.EpaoPrice)
                        {
                            // Identical apart from FromDate → keep last as-is
                        }
                        else
                        {
                            mergedCosts.Add(cost);
                        }
                    }
                }
            }

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

        return (breaks, mergedCosts);
    }

    public List<BreakInLearning> CalculateEnglishAndMathsBreaksInLearning(List<MathsAndEnglish> englishAndMathsItems)
    {
        throw new NotImplementedException();
    }
}