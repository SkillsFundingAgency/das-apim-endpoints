using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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
