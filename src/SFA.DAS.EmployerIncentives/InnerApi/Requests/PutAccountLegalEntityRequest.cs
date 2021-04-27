using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutAccountLegalEntityRequest : IPutApiRequest<AccountLegalEntityCreateRequest>
    {
        private readonly long _accountId;

        public PutAccountLegalEntityRequest(long accountId)
        {
            _accountId = accountId;
        }
        
        public string PutUrl => $"accounts/{_accountId}/legalentities";

        public AccountLegalEntityCreateRequest Data { get; set; }
    }

    public class AccountLegalEntityCreateRequest
    {
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string OrganisationName { get; set; }
    }
}