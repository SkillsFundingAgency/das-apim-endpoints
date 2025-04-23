using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.Enums;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyResponseV2 : GetVacanciesListResponseItemV2
    {
        /// <summary>
        /// A description of the company the apprentice will work at. Will be less than or equal to 4000 characters.
        /// </summary>
        [MaxLength(4000)]
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The apprenticeship�s training plan, including where and when training will take place.
        /// </summary>
        public string TrainingDescription { get ; set ; }
        /// <summary>
        /// Additional information about the training, such as details about the provider.
        /// </summary>
        public string AdditionalTrainingDescription { get; set; }
        /// <summary>
        /// What progress or outcome the apprentice can expect at the end of the apprenticeship.
        /// </summary>
        public string OutcomeDescription { get ; set ; }
        public string FullDescription { get ; set ; }
        public List<string> Skills { get ; set ; }
        public List<GetVacancyQualification> Qualifications { get ; set ; }
        public string ThingsToConsider { get; set; }
        public string CompanyBenefitsInformation { get; set; }

        public static implicit operator GetVacancyResponseV2(GetVacancyQueryResult source)
        {
            if (source.Vacancy == null)
            {
                return null;
            }
            return new GetVacancyResponseV2
            {
                AdditionalTrainingDescription = source.Vacancy.AdditionalTrainingDescription,
                Address = GetVacancyAddressItem.From(source.Vacancy.Address),
                ApprenticeshipLevel = source.Vacancy.ApprenticeshipLevel,
                ApplicationUrl = source.Vacancy.ApplicationUrl,
                ClosingDate = source.Vacancy.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)),
                CompanyBenefitsInformation = source.Vacancy.CompanyBenefitsInformation,
                Course = source.Vacancy,
                Description = source.Vacancy.Description,
                Distance = source.Vacancy.Distance,
                EmployerContactEmail = source.Vacancy.EmployerContactEmail,
                EmployerContactName = source.Vacancy.EmployerContactName,
                EmployerContactPhone = source.Vacancy.EmployerContactPhone,
                EmployerDescription = source.Vacancy.EmployerDescription,
                EmployerName = source.Vacancy.IsEmployerAnonymous ? source.Vacancy.AnonymousEmployerName : source.Vacancy.EmployerName,
                EmployerWebsiteUrl = source.Vacancy.EmployerWebsiteUrl,
                ExpectedDuration = source.Vacancy.ExpectedDuration,
                FullDescription = source.Vacancy.LongDescription,
                HoursPerWeek = source.Vacancy.HoursPerWeek,
                IsDisabilityConfident = source.Vacancy.IsDisabilityConfident,
                IsNationalVacancy = source.Vacancy.VacancyLocationType?.Equals("National", StringComparison.CurrentCultureIgnoreCase) ?? false,
                Location = !source.Vacancy.IsEmployerAnonymous ? new VacancyLocation { Lat = source.Vacancy.Location.Lat, Lon = source.Vacancy.Location.Lon } : null,
                NumberOfPositions = source.Vacancy.NumberOfPositions,
                OtherAddresses = source.Vacancy.OtherAddresses?.Select(GetVacancyAddressItem.From).ToList() ?? [],
                OutcomeDescription = source.Vacancy.OutcomeDescription,
                PostedDate = source.Vacancy.PostedDate,
                ProviderName = source.Vacancy.ProviderName,
                Qualifications = source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList(),
                Skills = source.Vacancy.Skills,
                StartDate = source.Vacancy.StartDate,
                ThingsToConsider = source.Vacancy.ThingsToConsider,
                Title = source.Vacancy.Title,
                TrainingDescription = source.Vacancy.TrainingDescription,
                Ukprn = int.Parse(source.Vacancy.Ukprn),
                VacancyReference = source.Vacancy.VacancySource.Equals(DataSource.Nhs) ? source.Vacancy.VacancyReference : source.Vacancy.VacancyReference.TrimVacancyReference(),
                VacancyUrl = source.Vacancy.VacancyUrl,
                Wage = source.Vacancy,
            };
        }
    }
}