using SFA.DAS.RoatpCourseManagement.Services.Models;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.NationalAchievementRatesLookup.Queries
{
    public class NationalAchievementRatesLookupQueryResult
    {
        public List<NationalAchievementRate> NationalAchievementRates { get; set; } = new List<NationalAchievementRate>();
        public List<NationalAchievementRateOverall> OverallAchievementRates { get; set; } = new List<NationalAchievementRateOverall>();
    }
}
