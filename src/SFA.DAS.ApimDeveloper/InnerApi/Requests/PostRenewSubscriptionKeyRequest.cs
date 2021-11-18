using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostRenewSubscriptionKeyRequest : IPostApiRequest<PostRenewSubscriptionKeyRequestBody>
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
        public PostRenewSubscriptionKeyRequestBody Data { get; set; }
    }
    
    public class PostRenewSubscriptionKeyRequestBody
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}