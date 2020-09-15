using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetLegalEntityRequest : IGetApiRequest
    {
        private readonly string _accountId;
        private readonly long _legalEntityId;

        public GetLegalEntityRequest(string accountId, long legalEntityId)
        {
            _accountId = accountId;
            _legalEntityId = legalEntityId;
        }

        public string GetUrl => $"api/accounts/{_accountId}/legalentities/{_legalEntityId}?includeAllAgreements=true";
    }
}