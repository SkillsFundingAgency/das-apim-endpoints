using SFA.DAS.SharedOuterApi.Interfaces;

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

        public string BaseUrl { get; set; }
        public string DeleteUrl => $"{BaseUrl}accounts/{_accountId}/legalentities/{_accountLegalEntityId}";
    }
}