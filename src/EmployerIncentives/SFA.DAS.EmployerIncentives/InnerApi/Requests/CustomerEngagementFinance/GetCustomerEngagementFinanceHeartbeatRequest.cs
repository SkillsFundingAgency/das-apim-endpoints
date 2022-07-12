using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CustomerEngagementFinance
{
    public class GetCustomerEngagementFinanceHeartbeatRequest : IGetApiRequest
    {
        public string GetUrl => "finance/heartbeat?api-version=2019-06-01";
    }
}