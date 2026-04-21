using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class DeleteAccountLegalEntityRequest : IDeleteApiRequest
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;

        public DeleteAccountLegalEntityRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string DeleteUrl => $"accounts/{_accountId}/legalentities/{_accountLegalEntityId}";
    }
}