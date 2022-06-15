using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class IncentiveApplication
    {
        public long LegalEntityId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string SubmittedByEmail { get; set; }
        public string SubmittedByName { get; set; }
        public bool BankDetailsRequired { get; set; }
        public bool NewAgreementRequired { get; set; }
        public IEnumerable<IncentiveApplicationApprenticeship> Apprenticeships { get; set; }
    }
}
