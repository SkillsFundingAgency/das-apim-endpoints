using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetTraineeshipVacancyResponse : GetTraineeshipVacanciesListResponseItem
    {
        public string WorkExperience { get; set; }
        public string FullDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public List<string> Skills { get; set; }
        public string ThingsToConsider { get; set; }

        public static implicit operator GetTraineeshipVacancyResponse(GetTraineeshipVacancyQueryResult source)
        {
            if (source.Vacancy == null)
            {
                return null;
            }
            return new GetTraineeshipVacancyResponse
            {
                Id = source.Vacancy.Id,
                ClosingDate = source.Vacancy.ClosingDate,
                Description = source.Vacancy.Description,
                AnonymousEmployerName = source.Vacancy.AnonymousEmployerName,
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
                Address = source.Vacancy,
                EmployerWebsiteUrl = source.Vacancy.EmployerWebsiteUrl,
                EmployerContactEmail = source.Vacancy.EmployerContactEmail,
                EmployerContactName = source.Vacancy.EmployerContactName,
                EmployerContactPhone = source.Vacancy.EmployerContactPhone,
                EmployerDescription = source.Vacancy.EmployerDescription,
                ExpectedDuration = source.Vacancy.ExpectedDuration,
                FullDescription = source.Vacancy.LongDescription,
                WorkExperience = source.Vacancy.WorkExperience,
                OutcomeDescription = source.Vacancy.OutcomeDescription,
                ThingsToConsider = source.Vacancy.ThingsToConsider,
                Skills = source.Vacancy.Skills,
                Distance = source.Vacancy.Distance,
                Location = !source.Vacancy.IsEmployerAnonymous ? new TraineeshipVacancyLocation
                {
                    Lat = source.Vacancy.Location.Lat,
                    Lon = source.Vacancy.Location.Lon
                } : null,
                RouteId = source.Vacancy.RouteId,
                RouteName = source.Vacancy.RouteName,
                IsEmployerAnonymous = source.Vacancy.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.Vacancy.IsPositiveAboutDisability,
                WorkingWeek = source.Vacancy.WorkingWeek,
                Score = source.Vacancy.Score,
                Category = source.Vacancy.Category,
                CategoryCode = source.Vacancy.CategoryCode
            };
        }
    }
}