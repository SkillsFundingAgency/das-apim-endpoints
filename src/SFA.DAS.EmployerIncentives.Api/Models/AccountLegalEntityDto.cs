using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class AccountLegalEntityDto
    {
        public long LegalEntityId { get ; set ; }

        public string LegalEntityName { get ; set ; }

        public long AccountLegalEntityId { get ; set ; }

        public long AccountId { get ; set ; }
        public bool HasSignedIncentivesTerms { get; set; }

        public static implicit operator AccountLegalEntityDto(AccountLegalEntity source)
        {
            return new AccountLegalEntityDto
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                LegalEntityName = source.LegalEntityName,
                LegalEntityId = source.LegalEntityId,
				HasSignedIncentivesTerms = source.HasSignedIncentivesTerms
            };
        }
    }
}