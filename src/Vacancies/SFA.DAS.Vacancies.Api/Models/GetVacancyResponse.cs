using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyResponse : GetVacanciesListResponseItem
    {
        /// <summary>
        /// A description of the company the apprentice will work at. Will be less than or equal to 4000 characters.
        /// </summary>
        [MaxLength(4000)]
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The apprenticeship’s training plan, including where and when training will take place.
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

        public static implicit operator GetVacancyResponse(GetVacancyQueryResult source)
        {
            if (source.Vacancy == null)
            {
                return null;
            }
            return new GetVacancyResponse
            {
                ClosingDate = source.Vacancy.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)),
                Description = source.Vacancy.Description,
                EmployerName = source.Vacancy.IsEmployerAnonymous ? source.Vacancy.AnonymousEmployerName : source.Vacancy.EmployerName,
                HoursPerWeek = source.Vacancy.HoursPerWeek,
                IsDisabilityConfident = source.Vacancy.IsDisabilityConfident,
                IsNationalVacancy = source.Vacancy.VacancyLocationType?.Equals("National", StringComparison.CurrentCultureIgnoreCase) ?? false,
                NumberOfPositions = source.Vacancy.NumberOfPositions,
                PostedDate = source.Vacancy.PostedDate,
                ProviderName = source.Vacancy.ProviderName,
                StartDate = source.Vacancy.StartDate,
                Title = source.Vacancy.Title,
                Ukprn = int.Parse(source.Vacancy.Ukprn),
                VacancyReference = source.Vacancy.VacancySource.Equals("NHS", StringComparison.CurrentCultureIgnoreCase) ? source.Vacancy.VacancyReference : source.Vacancy.VacancyReference.Replace("VAC",""),
                VacancyUrl = source.Vacancy.VacancyUrl,
                Course = source.Vacancy,
                Wage = source.Vacancy,
                Address = source.Vacancy,
                EmployerWebsiteUrl = source.Vacancy.EmployerWebsiteUrl,
                EmployerContactEmail = source.Vacancy.EmployerContactEmail,
                EmployerContactName = source.Vacancy.EmployerContactName,
                EmployerContactPhone = source.Vacancy.EmployerContactPhone,
                EmployerDescription = source.Vacancy.EmployerDescription,
                ApprenticeshipLevel = source.Vacancy.ApprenticeshipLevel,
                ExpectedDuration = source.Vacancy.ExpectedDuration,
                FullDescription = source.Vacancy.LongDescription,
                TrainingDescription = source.Vacancy.TrainingDescription,
                OutcomeDescription = source.Vacancy.OutcomeDescription,
                ThingsToConsider = source.Vacancy.ThingsToConsider,
                Skills = source.Vacancy.Skills,
                Qualifications = source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList(),
                Distance = source.Vacancy.Distance,
                Location = !source.Vacancy.IsEmployerAnonymous ? new VacancyLocation
                {
                    Lat = source.Vacancy.Location.Lat,
                    Lon = source.Vacancy.Location.Lon
                } : null,
                AdditionalTrainingDescription = source.Vacancy.AdditionalTrainingDescription,
                CompanyBenefitsInformation = source.Vacancy.CompanyBenefitsInformation,
            };
        }
    }
}