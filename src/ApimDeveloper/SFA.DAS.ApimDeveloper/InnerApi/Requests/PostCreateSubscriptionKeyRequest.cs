using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostCreateSubscriptionKeyRequest : IPostApiRequest
    {
        public PostCreateSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            Data = new CreateSubscriptionApiRequest
            {
                AccountIdentifier = accountIdentifier,
                ProductId = productId
            };
        }

        public string PostUrl => "api/subscription";
        public object Data { get; set; }
    }
    
    public class CreateSubscriptionApiRequest
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}