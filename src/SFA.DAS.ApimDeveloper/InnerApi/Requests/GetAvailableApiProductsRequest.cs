using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class GetAvailableApiProductsRequest : IGetApiRequest
    {
        private readonly string _accountType;

        public GetAvailableApiProductsRequest(string accountType)
        {
            _accountType = accountType;
        }

        public string GetUrl => $"api/products?group={_accountType}";
    }
}