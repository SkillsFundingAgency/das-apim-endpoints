using System;
using System.Collections.Generic;
using System.Linq;
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
                Duration = standard.ApprenticeshipFunding?.FirstOrDefault()?.Duration ?? 0,
                CourseType = ApprenticeshipCourseType.Standard,
                FundingPeriods = ConvertStandardFundingPeriod(standard.ApprenticeshipFunding)
            };
        }

        public static implicit operator ApprenticeshipCourse(GetFrameworksListItem framework)
        {
            return new ApprenticeshipCourse
            {
                Id = framework.Id,
                Title = framework.Title,
                FundingCap = framework.CurrentFundingCap,
                Level = framework.Level,
                Duration = framework.Duration,
                CourseType = ApprenticeshipCourseType.Framework,
                FundingPeriods = ConvertFrameworkFundingPeriod(framework.FundingPeriods)
            };
        }
        private static List<FundingPeriod> ConvertStandardFundingPeriod(List<GetStandardsListItem.FundingPeriod> fundingPeriod)
        {
            var fundingPeriods = new List<FundingPeriod>();
            foreach (var period in fundingPeriod)
            {
                fundingPeriods.Add(period);
            }

            return fundingPeriods;
        }

        private static List<FundingPeriod> ConvertFrameworkFundingPeriod(List<GetFrameworksListItem.FundingPeriod> fundingPeriod)
        {
            var fundingPeriods = new List<FundingPeriod>();
            foreach (var period in fundingPeriod)
            {
                fundingPeriods.Add(period);
            }

            return fundingPeriods;
        }
    }

    public class FundingPeriod
    {
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }

        public static implicit operator FundingPeriod(GetStandardsListItem.FundingPeriod fundingPeriod)
        {
            return new FundingPeriod
            {
                EffectiveFrom = fundingPeriod.EffectiveFrom,
                EffectiveTo = fundingPeriod.EffectiveTo,
                FundingCap = fundingPeriod.MaxEmployerLevyCap
            };
        }

        public static implicit operator FundingPeriod(GetFrameworksListItem.FundingPeriod fundingPeriod)
        {
            return new FundingPeriod
            {
                EffectiveFrom = fundingPeriod.EffectiveFrom,
                EffectiveTo = fundingPeriod.EffectiveTo,
                FundingCap = fundingPeriod.FundingCap
            };
        }
    }

    public enum ApprenticeshipCourseType
    {
        Standard = 1,
        Framework
    }
}