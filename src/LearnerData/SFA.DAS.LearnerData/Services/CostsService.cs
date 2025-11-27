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
            }
            return mergedCosts;
        }
    }
}
