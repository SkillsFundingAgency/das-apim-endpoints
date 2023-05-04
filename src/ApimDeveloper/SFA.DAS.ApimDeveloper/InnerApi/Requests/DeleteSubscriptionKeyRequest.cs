using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class DeleteSubscriptionKeyRequest : IDeleteApiRequest
    {
        private readonly string _accountIdentifier;
        private readonly string _productId;

        public DeleteSubscriptionKeyRequest(string accountIdentifier, string productId)
        {
            _accountIdentifier = accountIdentifier;
            _productId = productId;
        }

        public string DeleteUrl => $"api/subscription/{_accountIdentifier}/delete/{_productId}";
    }
}