using System;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class CourseDates
    {
        public DateTime? LastDateStarts { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime EffectiveFrom { get; set; }

        public static explicit operator CourseDates(GetStandardDates source)
        {
            return new CourseDates
            {
                LastDateStarts = source.LastDateStarts,
                EffectiveTo = source.EffectiveTo,
                EffectiveFrom = source.EffectiveFrom
            };
        }
    }
}