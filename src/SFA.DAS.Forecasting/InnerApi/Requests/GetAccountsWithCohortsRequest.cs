using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests
{
    public class GetAccountsWithCohortsRequest : IGetApiRequest
    {
        public string GetUrl => "api/cohorts/accountIds";
    }
}
