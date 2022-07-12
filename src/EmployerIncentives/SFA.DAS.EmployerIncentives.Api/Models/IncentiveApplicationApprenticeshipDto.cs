using SFA.DAS.EmployerIncentives.Models;
using System;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class IncentiveApplicationApprenticeshipDto
    {
        public long ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
        public decimal TotalIncentiveAmount { get; set; }
        public long Uln { get; set; }
        public DateTime PlannedStartDate { get; set; }

        public DateTime? EmploymentStartDate { get; set; }

        public bool StartDatesAreEligible { get; set; }

        public static implicit operator IncentiveApplicationApprenticeshipDto(IncentiveApplicationApprenticeship source)
        {
            return new IncentiveApplicationApprenticeshipDto
            {
                ApprenticeshipId = source.ApprenticeshipId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                TotalIncentiveAmount = source.TotalIncentiveAmount,
                CourseName = source.CourseName,
                Uln = source.Uln,
                PlannedStartDate = source.PlannedStartDate,
                EmploymentStartDate = source.EmploymentStartDate,
                StartDatesAreEligible = source.StartDatesAreEligible
            };
        }
    }
}