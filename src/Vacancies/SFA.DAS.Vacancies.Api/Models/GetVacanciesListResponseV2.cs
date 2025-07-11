using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public record GetVacanciesListResponseV2
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
    
    public record GetVacanciesListResponseItemV2 : GetVacanciesListResponseItem
    {
        /// <summary>
        /// The web address for the apprentice will apply.
        /// </summary>
        public string ApplicationUrl { get; set; }
        /// <summary>
        /// The address of where the apprentice will work.
        /// If the address is available at more than one location, you'll get an address and otherAddresses.If you included a lat and long when requesting, these will be returned in order of distance.
        /// If isNationalVacancy is true, there will be no address provided. 
        /// </summary>
        public List<GetVacancyAddressItem> Addresses { get; set; }

        [JsonIgnore] 
        public new List<GetVacancyAddressItem> OtherAddresses { get; set; }
        [JsonIgnore]
        public new VacancyLocation Location { get; set; }

        public static implicit operator GetVacanciesListResponseItemV2(GetVacanciesListItem source)
        {
            var isRecruitNationally = source.VacancyLocationType != null &&
                                      source.VacancyLocationType.Equals("National",
                                          StringComparison.CurrentCultureIgnoreCase);

            return new GetVacanciesListResponseItemV2
            {
                ClosingDate = source.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)),
                Description = source.Description,
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsNationalVacancy = isRecruitNationally,
                IsNationalVacancyDetails = isRecruitNationally ? source.EmploymentLocationInformation : string.Empty,
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
                Addresses = source.OtherAddresses?.Select(GetVacancyAddressItem.From).ToList() ?? [],
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                ApplicationUrl = source.ApplicationUrl,
                ExpectedDuration = source.ExpectedDuration,
            };
        }
    }
}
