using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck
{
    public class EarningsResilenceCheckRequest : IPostApiRequest
    {
        public EarningsResilenceCheckRequest()
        {
        }

        public string PostUrl => $"earnings-resilience-check";
        public object Data { get; set; }
    
    }
}
