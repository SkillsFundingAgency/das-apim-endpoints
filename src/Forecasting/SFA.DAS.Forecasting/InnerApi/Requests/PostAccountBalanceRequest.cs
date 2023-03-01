using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests
{
    public class PostAccountBalanceRequest : IPostApiRequest
    {
        private readonly string _accountId;

        public PostAccountBalanceRequest(string accountId)
        {
            _accountId = accountId;
            Data = new List<string> {_accountId};
        }
        public string PostUrl => $"api/accounts/balances";
        public object Data { get; set; }
    }
}