using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PutAccountLegalEntityRequest : IPutApiRequest<AccountLegalEntityCreateRequest>
    {
        public string PutUrl => $"accounts/{Data.AccountId}/legalentities";

        public AccountLegalEntityCreateRequest Data { get; set; }
    }

    public class AccountLegalEntityCreateRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string OrganisationName { get; set; }
    }
}