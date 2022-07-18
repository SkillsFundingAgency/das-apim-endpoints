using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetAccountsApiPingRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}