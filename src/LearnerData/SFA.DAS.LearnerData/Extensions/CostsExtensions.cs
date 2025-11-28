using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Extensions;

public static class CostsExtensions
{
    public static List<Cost> GetCostsOrDefault(this List<CostDetails>? costs, DateTime startDate)
    {
        if (costs == null || costs.Count == 0)
        {
            return
            [
                new Cost
                {
                    TrainingPrice = 0,
                    EpaoPrice = null,
                    FromDate = startDate
                }
            ];
        }

        return costs.Select(x => new Cost
        {
            TrainingPrice = x.TrainingPrice ?? 0,
            EpaoPrice = x.EpaoPrice,
            FromDate = x.FromDate ?? startDate
        }).ToList();
    }
}
