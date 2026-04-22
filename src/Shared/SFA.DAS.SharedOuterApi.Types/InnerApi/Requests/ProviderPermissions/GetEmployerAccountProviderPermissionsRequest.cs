using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderPermissions
{
    public class GetEmployerAccountProviderPermissionsRequest : IGetApiRequest
    {
        private readonly string _hashedAccountId;

        public GetEmployerAccountProviderPermissionsRequest(string hashedAccountId)
        {
            _hashedAccountId = hashedAccountId;
        }

        public string GetUrl => $"accountproviderlegalentities?accountHashedId={_hashedAccountId}&operations=1&operations=2";
    }
}