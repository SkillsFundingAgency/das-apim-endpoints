using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class BankingData
    {
        public long LegalEntityId { get; set; }
        public string VendorCode { get; set; }
        public string SubmittedByName { get; set; }
        public string SubmittedByEmail { get; set; }
        public decimal ApplicationValue { get; set; }
        public int NumberOfApprenticeships { get; set; }
        public IEnumerable<SignedAgreement> SignedAgreements { get; set; }
    }
}
