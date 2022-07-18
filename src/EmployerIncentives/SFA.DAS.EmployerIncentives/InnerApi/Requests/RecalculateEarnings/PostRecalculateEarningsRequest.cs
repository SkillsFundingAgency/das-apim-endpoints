using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings
{
    public class PostRecalculateEarningsRequest : IPostApiRequest
    {
        public PostRecalculateEarningsRequest(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            Data = recalculateEarningsRequest;
        }

        public string PostUrl => "earningsRecalculations";

        public object Data { get; set; }
    }
}
