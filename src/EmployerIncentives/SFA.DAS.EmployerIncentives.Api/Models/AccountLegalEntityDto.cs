using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class AccountLegalEntityDto
    {
        public long LegalEntityId { get ; set ; }

        public string LegalEntityName { get ; set ; }

        public long AccountLegalEntityId { get ; set ; }

        public long AccountId { get ; set ; }

        public string VrfVendorId { get; set; }

        public string VrfCaseStatus { get; set; }

        public string HashedLegalEntityId { get; set; }

        public bool IsAgreementSigned { get; set; }
        public bool BankDetailsRequired { get; set; }

        public static implicit operator AccountLegalEntityDto(AccountLegalEntity source)
        {
            return new AccountLegalEntityDto
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                LegalEntityName = source.LegalEntityName,
                LegalEntityId = source.LegalEntityId,
                IsAgreementSigned = source.IsAgreementSigned,
                VrfVendorId = source.VrfVendorId,
                VrfCaseStatus = source.VrfCaseStatus,
                HashedLegalEntityId = source.HashedLegalEntityId,
                BankDetailsRequired = source.BankDetailsRequired
            };
        }
    }
}