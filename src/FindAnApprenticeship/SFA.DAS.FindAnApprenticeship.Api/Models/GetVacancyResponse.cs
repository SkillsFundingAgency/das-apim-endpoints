using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetVacancyResponse : GetVacanciesListResponseItem
    {
        public string TrainingDescription { get ; set ; }
        public string FullDescription { get ; set ; }
        public string OutcomeDescription { get ; set ; }
        public string EmployerDescription { get; set; }
        public List<string> Skills { get ; set ; }
        public List<GetVacancyQualification> Qualifications { get ; set ; }
        public string ThingsToConsider { get; set; }

        public static implicit operator GetVacancyResponse(GetVacancyQueryResult source)
        {
            if (source.Vacancy == null)
            {
                return null;
            }
            return new GetVacancyResponse
            {
                ClosingDate = source.Vacancy.ClosingDate,
                Description = source.Vacancy.Description,
                EmployerName = source.Vacancy.IsEmployerAnonymous ? source.Vacancy.AnonymousEmployerName : source.Vacancy.EmployerName,
                HoursPerWeek = source.Vacancy.HoursPerWeek,
                IsDisabilityConfident = source.Vacancy.IsDisabilityConfident,
                IsNationalVacancy = source.Vacancy.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase),
                NumberOfPositions = source.Vacancy.NumberOfPositions,
                PostedDate = source.Vacancy.PostedDate,
                ProviderName = source.Vacancy.ProviderName,
                StartDate = source.Vacancy.StartDate,
                Title = source.Vacancy.Title,
                Ukprn = source.Vacancy.Ukprn,
                VacancyReference = source.Vacancy.VacancyReference,
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
                } : null
            };
        }
    }
}