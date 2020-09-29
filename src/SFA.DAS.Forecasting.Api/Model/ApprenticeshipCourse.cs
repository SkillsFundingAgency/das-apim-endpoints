using System;
using System.Collections.Generic;

namespace SFA.DAS.Forecasting.Api.Model
{
    public class ApprenticeshipCourse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal FundingCap { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public ApprenticeshipCourseType CourseType { get; set; }
        public List<FundingPeriod> FundingPeriods { get; set; }
    }

    public class FundingPeriod
    {
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }
    }

    public enum ApprenticeshipCourseType
    {
        Standard = 1,
        Framework
    }
}