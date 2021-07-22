namespace SFA.DAS.EmployerIncentives.Models
{
    public class AccountLegalEntity
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string VrfVendorId { get; set; }
        public string VrfCaseStatus { get; set; }
        public string HashedLegalEntityId { get; set; }
        public bool IsAgreementSigned { get; set; }
        public bool BankDetailsRequired { get; set; }
    }
}