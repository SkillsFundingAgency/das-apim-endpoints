namespace SFA.DAS.EmployerIncentives.Models
{
    public class AccountLegalEntity
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasSignedIncentivesTerms { get; set; }
        public string VrfCaseStatus { get; set; }

    }
}