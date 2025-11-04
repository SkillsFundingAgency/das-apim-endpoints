using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Extensions
{
    public static class CostsExtensions
    {
        public static List<Cost> MapCosts(this OnProgrammeRequestDetails source)
        {
            if (source.Costs == null || source.Costs.Count==0)
            {
                return
                [
                    new Cost
                    {
                        TrainingPrice = 0,
                        EpaoPrice = null,
                        FromDate = source.StartDate
                    }
                ];
            }

            return source.Costs.Select(x => new Cost
            {
                TrainingPrice = x.TrainingPrice ?? 0,
                EpaoPrice = x.EpaoPrice,
                FromDate = x.FromDate ?? source.StartDate
            }).ToList();
        }
    }
}
