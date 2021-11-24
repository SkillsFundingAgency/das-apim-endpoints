using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostRenewSubscriptionKeyRequest : IPostApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;

        public PostRenewSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
        }

        public string PostUrl => $"api/subscription/{_accountIdentifier}/renew/{_productId}";
        public object Data { get; set; }
    }
}