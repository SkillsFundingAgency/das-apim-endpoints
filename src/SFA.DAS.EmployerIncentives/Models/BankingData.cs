using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class BankingData
    {
        public long LegalEntityId { get; set; }
        public string VendorCode { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public decimal ApplicationValue { get; set; }
        public IEnumerable<SignedAgreement> SignedAgreements { get; set; }
    }
}
