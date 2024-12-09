using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.VacancyServices.Wage;
using WageType = SFA.DAS.FindAnApprenticeship.Domain.Models.WageType;

namespace SFA.DAS.FindAnApprenticeship.Domain;

public static class WageExtensions
    {
        public static string ToDisplayText(this Wage wage, DateTime? expectedStartDate)
        {
            var wageDetails = new WageDetails
            {
                Amount = wage.FixedWageYearlyAmount,
                HoursPerWeek = wage.WeeklyHours,
                StartDate = expectedStartDate.GetValueOrDefault()
            };

            return (WageType)wage.WageType switch
            {
                WageType.FixedWage => WagePresenter
                    .GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.Custom, WageUnit.Annually, wageDetails)
                    .AsWholeMoneyAmountPerYear(),
                WageType.NationalMinimumWage => WagePresenter
                    .GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.NationalMinimum, WageUnit.Annually, wageDetails)
                    .AsWholeMoneyAmountPerYear(),
                WageType.NationalMinimumWageForApprentices => WagePresenter
                    .GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.ApprenticeshipMinimum, WageUnit.Annually, wageDetails)
                    .AsWholeMoneyAmountPerYear(),
                WageType.CompetitiveSalary => "Competitive",
                _ => wage.WageType.ToString()
            };
        }
        
        private static string AsWholeMoneyAmount(this string value)
        {
            return value.Replace(".00", "");
        }
        
        private static string AsPerYear(this string value)
        {
            return $"{value} a year";
        }
        
        private static string AsWholeMoneyAmountPerYear(this string value)
        {
            return value.AsWholeMoneyAmount().AsPerYear();
        }
    }