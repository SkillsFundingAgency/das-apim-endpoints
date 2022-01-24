using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

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
                Course = new VacancyCourseItem
                {
                    LarsCode = source.Vacancy.StandardLarsCode ?? 0,
                    Level = source.Vacancy.CourseLevel,
                    Route = source.Vacancy.Route,
                    Title = $"{source.Vacancy.CourseTitle} (level {source.Vacancy.CourseLevel})"
                },
                Wage = new VacancyWageItem
                {
                    WageAmount = source.Vacancy.WageAmount,
                    WageType = (WageType)source.Vacancy.WageType,
                    WageAdditionalInformation = source.Vacancy.WageText,
                    WorkingWeekDescription = source.Vacancy.WorkingWeek,
                    WageAmountLowerBound = source.Vacancy.WageAmountLowerBound,
                    WageAmountUpperBound = source.Vacancy.WageAmountUpperBound
                },
                FullDescription = source.Vacancy.LongDescription,
                TrainingDescription = source.Vacancy.TrainingDescription,
                OutcomeDescription = source.Vacancy.OutcomeDescription,
                Skills = source.Vacancy.Skills,
                Qualifications = source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList()
            };
        }
    }

    public class GetVacancyQualification
    {
        public QualificationWeighting Weighting { get ; set ; }
        public string QualificationType { get ; set ; }
        public string Subject { get ; set ; }
        public string Grade { get ; set ; }
        
        public static implicit operator GetVacancyQualification(GetVacancyQualificationResponseItem source)
        {
            return new GetVacancyQualification
            {
                QualificationType = source.QualificationType,
                Grade = source.Grade,
                Subject = source.Subject,
                Weighting = (QualificationWeighting)source.Weighting
            };
        }

        public enum QualificationWeighting
        {
            Essential,
            Desired
        }
    }
}