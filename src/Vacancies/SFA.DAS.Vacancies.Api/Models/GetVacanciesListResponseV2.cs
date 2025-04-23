using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacanciesListResponseV2
    {
        public List<GetVacanciesListResponseItemV2> Vacancies { get; set; }
        public long Total { get ; set ; }
        public long TotalFiltered { get ; set ; }
        public int TotalPages { get ; set ; }

        public static implicit operator GetVacanciesListResponseV2(GetVacanciesQueryResult source)
        {
            return new GetVacanciesListResponseV2
            {
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItemV2)c).ToList(),
                Total = source.Total,
                TotalFiltered = source.TotalFiltered,
                TotalPages = source.TotalPages
            };
        }
    }
    
    public class GetVacanciesListResponseItemV2 : GetVacanciesListResponseItem
    {
        /// <summary>
        /// The web address for the apprentice will apply.
        /// </summary>
        public string ApplicationUrl { get; set; }
        
        public static implicit operator GetVacanciesListResponseItemV2(GetVacanciesListItem source)
        {
            return new GetVacanciesListResponseItemV2
            {
                ClosingDate = source.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)),
                Description = source.Description,
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsNationalVacancy = source.VacancyLocationType != null && source.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase),
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                StartDate = source.StartDate,
                Title = source.Title,
                Ukprn = int.Parse(source.Ukprn),
                VacancyReference = source.VacancyReference.Replace("VAC",""),
                VacancyUrl = source.VacancyUrl,
                Course = source,
                Wage = source,
                Distance = source.Distance,
                Address = GetVacancyAddressItem.From(source.Address),
                OtherAddresses = source.OtherAddresses?.Select(GetVacancyAddressItem.From).ToList() ?? [],
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                ApplicationUrl = source.ApplicationUrl,
                ExpectedDuration = source.ExpectedDuration,
                Location = new VacancyLocation
                {
                    Lat = source.Location.Lat,
                    Lon = source.Location.Lon
                }
            };
        }
    }
}
