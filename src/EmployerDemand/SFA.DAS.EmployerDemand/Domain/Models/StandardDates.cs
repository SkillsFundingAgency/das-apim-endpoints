using System;

namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class StandardDates
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}