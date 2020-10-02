using System;
using System.Collections.Generic;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.Api.Models
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

        public static implicit operator ApprenticeshipCourse(GetStandardsListItem standard)
        {
            return new ApprenticeshipCourse
            {
                Id = standard.Id,
                Title = standard.Title,
                FundingCap = standard.FundingCap,
                Level = standard.Level,
                Duration = standard.Duration,
                CourseType = ApprenticeshipCourseType.Standard,
                //FundingPeriods = standard.FundingPeriods
            };
        }

        public static implicit operator ApprenticeshipCourse(GetFrameworksListItem framework)
        {
            return new ApprenticeshipCourse
            {
                Id = framework.Id,
                Title = framework.Title,
                FundingCap = framework.FundingCap,
                Level = framework.Level,
                Duration = framework.Duration,
                CourseType = ApprenticeshipCourseType.Framework,
                //FundingPeriods = framework.FundingPeriod
            };
        }
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