using System;

namespace SFA.DAS.EmployerIncentives.Models.EmployerIncentives
{
    public class IncentiveClaimApprenticeshipDto
    {
        public long ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long Uln { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerTypeOnApproval { get; set; }
    }
}
