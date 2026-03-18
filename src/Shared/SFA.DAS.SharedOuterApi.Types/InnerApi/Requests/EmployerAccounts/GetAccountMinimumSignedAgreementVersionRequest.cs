using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts
{
    public class GetAccountMinimumSignedAgreementVersionRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountMinimumSignedAgreementVersionRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{AccountId}/minimum-signed-agreement-version";
    }
}
