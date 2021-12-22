using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacanciesListResponse 
    {
        public List<GetVacanciesListResponseItem> Vacancies { get; set; }

        public static implicit operator GetVacanciesListResponse(GetVacanciesQueryResult source)
        {
            return new GetVacanciesListResponse()
            {
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItem)c).ToList()
            };
        }

        public class GetVacanciesListResponseItem
       {
           public string ApprenticeshipLevel { get; set; }
           public DateTimeOffset ClosingDate { get; set; }
           public string Description { get; set; }
           public string EmployerName { get; set; }
           public long HoursPerWeek { get; set; }
           public bool IsDisabilityConfident { get; set; }
           public  bool IsNationalVacancy { get; set; }
           public long NumberOfPositions { get; set; }
           public DateTimeOffset PostedDate { get; set; }
           public string ProviderName { get; set; }
           public int StandardLarsCode { get; set; }
           public DateTimeOffset StartDate { get; set; }
           public string Title { get; set; }
           public int Ukprn { get; set; }
           public long VacancyReference { get; set; }
           public string WageAmount { get; set; }
           public string WageAmountLowerBound { get; set; }
           public string WageAmountUpperBound { get; set; }
           public string WageAdditionalInformation { get; set; }
           public long WageType { get; set; }
           public string WorkingWeekDescription { get; set; }
           public string EmploymentDuration { get; set; }
           public string EmploymentDurationUnit { get; set; }
           public long Route { get; set; }

           public static implicit operator GetVacanciesListResponseItem(GetVacanciesItem source)
           {
               return new GetVacanciesListResponseItem()
               {
                   ApprenticeshipLevel = source.ApprenticeshipLevel,
                   ClosingDate = source.ClosingDate,
                   Description = source.Description,
                   EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                   HoursPerWeek = source.HoursPerWeek,
                   IsDisabilityConfident = source.IsDisabilityConfident,
                   IsNationalVacancy = source.VacancyLocationType == "National" ? true : false,
                   NumberOfPositions = source.NumberOfPositions,
                   PostedDate = source.PostedDate,
                   ProviderName = source.ProviderName,
                   Route = source.Score,
                   StandardLarsCode = source.StandardLarsCode,
                   StartDate = source.StartDate,
                   Title = source.Title,
                   Ukprn = source.Ukprn,
                   VacancyReference = source.VacancyReference,
                   WageAmount = source.WageAmount,
                   WageAmountLowerBound = source.WageAmountLowerBound,
                   WageAmountUpperBound = source.WageAmountUpperBound,
                   WageAdditionalInformation = source.WageText,
                   WageType = source.WageType,
                   WorkingWeekDescription = source.WorkingWeek,
                   EmploymentDuration = source.EmploymentDuration,
                   EmploymentDurationUnit = source.EmploymenrDurationUnit,
               };
           }
        }
    }
}
