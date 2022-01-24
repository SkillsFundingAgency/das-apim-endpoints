using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyResponse : GetVacanciesListResponseItem
    {
        public string TrainingDescription { get ; set ; }
        public string FullDescription { get ; set ; }
        public string OutcomeDescription { get ; set ; }
        public List<string> Skills { get ; set ; }
        public List<GetVacancyQualification> Qualifications { get ; set ; }

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
                FullDescription = source.Vacancy.LongDescription,
                TrainingDescription = source.Vacancy.TrainingDescription,
                OutcomeDescription = source.Vacancy.OutcomeDescription,
                Skills = source.Vacancy.Skills,
                Qualifications = source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList(),
                Location = !source.Vacancy.IsEmployerAnonymous ? new VacancyLocation
                {
                    Lat = source.Vacancy.Location.Lat,
                    Lon = source.Vacancy.Location.Lon
                } : null
            };
        }
    }
}