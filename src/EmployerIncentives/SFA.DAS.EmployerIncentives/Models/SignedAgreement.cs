using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class SignedAgreement
    {
        public DateTime SignedDate { get; set; }
        public string SignedByName { get; set; }
        public string SignedByEmail { get; set; }
    }
}
