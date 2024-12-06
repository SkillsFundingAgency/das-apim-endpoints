using SFA.DAS.Vacancies.InnerApi.Responses;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyWageItem
    {
        public WageType WageType { get; set; }
        /// <summary>
        /// If `wageType` is `Custom`, this will be the set annual wage for the apprenticeship.
        /// </summary>
        public decimal? WageAmount { get; set; }
        public WageUnit WageUnit { get; set; }
        /// <summary>
        /// Additional information about pay, such as when the apprentice might get a pay rise. Will be less than or equal to 250 characters.
        /// </summary>
        [MaxLength(250)]
        public string WageAdditionalInformation { get; set; }
        /// <summary>
        /// Information about the working schedule, such as daily working hours. Will be less than or equal to 250 characters.
        /// </summary>
        [MaxLength(250)]
        public string WorkingWeekDescription { get; set; }

        public static implicit operator GetVacancyWageItem(GetVacanciesListItem source)
        {
            return new GetVacancyWageItem
            {
                WageAmount = source.WageAmount,
                WageType = source.WageType == 0 ? WageType.Custom : (WageType)source.WageType,
                WageAdditionalInformation = source.WageText,
                WorkingWeekDescription = source.WorkingWeek,
                WageUnit = (WageUnit)source.WageUnit
            };
        }
    }
}