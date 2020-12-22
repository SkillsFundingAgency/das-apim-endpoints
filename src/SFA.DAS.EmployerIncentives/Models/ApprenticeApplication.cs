using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class ApprenticeApplication
    {
        public long AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public string LegalEntityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long ULN { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal TotalIncentiveAmount { get; set; }
        public string Status { get; set; }
    }
}
