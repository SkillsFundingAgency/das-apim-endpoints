using System;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class CourseDates
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public static implicit operator CourseDates(StandardDates source)
        {
            return new CourseDates
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LastDateStarts = source.LastDateStarts
            };  
        }
    }
}