using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class AccountLegalEntityDto
    {
        public long LegalEntityId { get ; set ; }

        public string OrganisationName { get ; set ; }

        public long AccountLegalEntityId { get ; set ; }

        public long AccountId { get ; set ; }

        public static implicit operator AccountLegalEntityDto(AccountLegalEntity source)
        {
            return new AccountLegalEntityDto
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                OrganisationName = source.LegalEntityName,
                LegalEntityId = source.LegalEntityId
            };
        }
    }
}