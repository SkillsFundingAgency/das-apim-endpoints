using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships
{
    public class GetAccountProvidersRequest : IGetApiRequest
    {
        private readonly long _accountId;
        public GetAccountProvidersRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"accounts/{_accountId}/providers";
    }
}