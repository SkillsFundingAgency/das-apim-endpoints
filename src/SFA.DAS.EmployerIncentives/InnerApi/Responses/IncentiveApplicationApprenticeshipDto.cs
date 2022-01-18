using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class IncentiveApplicationApprenticeshipDto
    {
        public int ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalIncentiveAmount { get; set; }
        public long Uln { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime? EmploymentStartDate { get; set; }        
        public bool StartDatesAreEligible { get; set; }
    }
}
