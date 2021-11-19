using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostRenewSubscriptionKeyRequest : IPostApiRequest
    {
        public PostRenewSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            Data = new PostRenewSubscriptionKeyRequestBody
            {
                AccountIdentifier = accountIdentifier,
                ProductId = productId
            };
        }

        public string PostUrl => "api/subscription/renew";
        public object Data { get; set; }
    }
    
    public class PostRenewSubscriptionKeyRequestBody
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}