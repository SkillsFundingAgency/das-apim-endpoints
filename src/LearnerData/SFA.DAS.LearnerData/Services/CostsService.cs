using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Services
{
    public interface ICostsService
    {
        List<CostDetails> GetCosts(List<OnProgrammeRequestDetails> onProgrammeItems);
    }

    public class CostsService : ICostsService
    {
        public List<CostDetails> GetCosts(List<OnProgrammeRequestDetails> onProgrammeItems)
        {
            var mergedCosts = new List<CostDetails>();

            foreach (var current in onProgrammeItems)
            {
                // Merge costs: union all, but discard redundant consecutive entries
                if (current.Costs == null) continue;
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
                            // Identical apart from FromDate keep previous as-is and skip this one
                        }
                        else
                        {
                            mergedCosts.Add(cost);
                        }
                    }
                }
            }
            return mergedCosts;
        }
    }
}
