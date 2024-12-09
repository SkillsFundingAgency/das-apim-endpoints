using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests;

public class PostAccountBalanceRequest(string accountId) : IPostApiRequest
{
    public string PostUrl => "api/accounts/balances";
    public object Data { get; set; } = new List<string> { accountId };
}