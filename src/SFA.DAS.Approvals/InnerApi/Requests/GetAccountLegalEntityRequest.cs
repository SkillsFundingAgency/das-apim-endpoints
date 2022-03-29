using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountLegalEntityRequest : IGetApiRequest
    {
        public long _accountLegalEntityId;
        public string GetUrl => $"api/AccountLegalEntity/{_accountLegalEntityId}";

        public GetAccountLegalEntityRequest(long accountLegalEntityId)
        {
            _accountLegalEntityId = accountLegalEntityId;
        }
    }
}
