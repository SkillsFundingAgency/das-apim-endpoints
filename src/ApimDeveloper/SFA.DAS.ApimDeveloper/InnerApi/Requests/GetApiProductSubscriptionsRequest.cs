using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class GetApiProductSubscriptionsRequest : IGetApiRequest
    {
        private readonly string _accountIdentifier;

        public GetApiProductSubscriptionsRequest(string accountIdentifier)
        {
            _accountIdentifier = accountIdentifier;
        }

        public string GetUrl => $"api/subscription/{_accountIdentifier}";
    }
}