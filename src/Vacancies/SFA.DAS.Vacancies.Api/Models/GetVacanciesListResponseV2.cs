using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies;
using SFA.DAS.Vacancies.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
    
    public record GetVacanciesListResponseItemV2
    {
        /// <summary>
        /// The title for the apprenticeship vacancy. Will be less than or equal to 100 characters.
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// A short summary of the overall apprenticeship. Will be less than or equal to 350 characters.
        /// </summary>
        [MaxLength(350)]
        public string Description { get; set; }
        /// <summary>
        /// The number of apprentices being recruited for the apprenticeship. Will be 1 or higher.
        /// </summary>
        public long NumberOfPositions { get; set; }
        /// <summary>
        /// The date the apprenticeship was added to Find an apprenticeship.
        /// </summary>
        /// <example>2019-08-24T:14:15:22Z</example>
        public DateTime PostedDate { get; set; }
        /// <summary>
        /// The last date people can apply for the apprenticeship. Will always be more than 2 weeks after the posted date.
        /// </summary>
        /// <example>2019-08-24T:14:15:22Z</example>
        public DateTime ClosingDate { get; set; }
        /// <summary>
        /// The date the company plans for the apprenticeship to start.
        /// </summary>
        /// <example>2019-08-24T:14:15:22Z</example>
        public DateTime StartDate { get; set; }
        public GetVacancyWageItem Wage { get; set; }
        /// <summary>
        /// The total number of hours per week. This includes both work and training.
        /// </summary>
        public decimal HoursPerWeek { get; set; }
        /// <summary>
        /// How long the apprenticeship will be. Will be in months, written as a string of text.
        /// </summary>
        /// <example>21 months</example>
        public string ExpectedDuration { get; set; }

        /// <summary>
        /// The address of where the apprentice will work.
        /// If the address is available at more than one location, you'll get an address and otherAddresses.If you included a lat and long when requesting, these will be returned in order of distance.
        /// If isNationalVacancy is true, there will be no address provided. 
        /// </summary>
        public List<GetVacancyAddressItem> Addresses { get; set; }

        /// <summary>
        /// The web address for the apprentice will apply.
        /// </summary>
        public string ApplicationUrl { get; set; }

        /// <summary>
        /// If you provide a `lat` and `lon` for a location when using `GET list of vacancies` or `GET vacancy by reference number`, this will be the distance between the apprenticeship and your defined location. Will be in miles.
        /// </summary>
        public decimal? Distance { get; set; }
        /// <summary>
        /// Use this to get vacancies from a specific employer. The name does not need to be exact, as we’ll match it to the closest employer we have adverts for.
        /// </summary>
        public string EmployerName { get; set; }
        /// <summary>
        /// The web address for the company the apprentice will work at.
        /// </summary>
        public string EmployerWebsiteUrl { get; set; }
        /// <summary>
        /// A named contact a person can use to ask questions about the apprenticeship before applying. This contact can be from either the employer or training provider.
        /// </summary>
        public string EmployerContactName { get; set; }
        /// <summary>
        /// A phone number a person can use to ask questions about the apprenticeship before applying. This contact can be from either the employer or training provider.
        /// </summary>
        public string EmployerContactPhone { get; set; }
        /// <summary>
        /// An email address a person can use to ask questions about the apprenticeship before applying. This contact can be from either the employer or training provider.
        /// </summary>
        public string EmployerContactEmail { get; set; }
        public GetVacancyCourseItem Course { get; set; }
        /// <summary>
        /// Whether an apprenticeship is ‘intermediate’ (level 2), ‘advanced’ (level 3), ‘higher’ (level 4 to 5) or ‘degree’ (level 6 or 7).
        /// </summary>
        public string ApprenticeshipLevel { get; set; }
        /// <summary>
        /// The name of the apprenticeship’s training provider.
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// The UK provider reference number (UKRPN) for the apprenticeship’s training provider.
        /// </summary>
        public int Ukprn { get; set; }
        /// <summary>
        /// Says whether the employer is part of the Department for Work and Pension’s Disability Confident scheme.
        /// </summary>
        public bool IsDisabilityConfident { get; set; }
        /// <summary>
        /// The address for the apprenticeship’s vacancy on Find an apprenticeship.
        /// </summary>
        public string VacancyUrl { get; set; }
        /// <summary>
        /// The unique reference code for the vacancy on Find an apprenticeship.
        /// </summary>
        public string VacancyReference { get; set; }
        /// <summary>
        /// If the apprenticeship is available to applicants across the entirety of England. For example, if the apprenticeship is available in many locations across England, remote working or provides live-in accommodation. When isNationalVacancy is true, there will be no address. When true, there will be isNationalVacancyDetails.
        /// </summary>
        public bool IsNationalVacancy { get; set; }

        /// <summary>
        /// Only provided when isNationalVacancy is true. Includes details about why an apprenticeship is recruiting nationally and where an apprentice will work. 
        /// </summary>
        [MaxLength(500)]
        public string IsNationalVacancyDetails { get; set; }


        public static implicit operator GetVacanciesListResponseItemV2(GetVacanciesListItem source)
        {
            var addresses = new List<GetVacancyAddressItem>();
            if (source.Address != null)
            {
                addresses.Add(GetVacancyAddressItem.From(source.Address));
            }
            if (source.OtherAddresses != null && source.OtherAddresses.Any())
            {
                addresses.AddRange(source.OtherAddresses.Select(GetVacancyAddressItem.From));
            }

            var isRecruitNationally = string.Equals(
                source.VacancyLocationType,
                "National",
                StringComparison.CurrentCultureIgnoreCase);

            return new GetVacanciesListResponseItemV2
            {
                ClosingDate = source.ClosingDate.AddDays(1).AddSeconds(-1),
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
                Ukprn = int.TryParse(source.Ukprn, out var ukprn) ? ukprn : 0,
                VacancyReference = source.VacancyReference?.Replace("VAC", "") ?? string.Empty,
                VacancyUrl = source.VacancyUrl,
                Course = source,
                Wage = source,
                Distance = source.Distance,
                Addresses = addresses,
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