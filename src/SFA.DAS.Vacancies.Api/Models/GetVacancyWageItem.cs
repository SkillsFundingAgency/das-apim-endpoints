using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyWageItem
    {
        public decimal? WageAmount { get; set; }
        public decimal? WageAmountLowerBound { get; set; }
        public decimal? WageAmountUpperBound { get; set; }
        public string WageAdditionalInformation { get; set; }
        public WageType WageType { get; set; }
        public string WorkingWeekDescription { get; set; }
        public WageUnit WageUnit { get; set; }

        public static implicit operator GetVacancyWageItem(GetVacanciesListItem source)
        {
            return new GetVacancyWageItem
            {
                WageAmount = source.WageAmount,
                WageType = (WageType)source.WageType,
                WageAdditionalInformation = source.WageText,
                WorkingWeekDescription = source.WorkingWeek,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageUnit = (WageUnit)source.WageUnit
            };
        }
    }
}