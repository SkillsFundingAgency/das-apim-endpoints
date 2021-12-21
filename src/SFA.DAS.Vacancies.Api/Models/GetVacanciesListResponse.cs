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
           public long Id { get; set; }
           public string ApprenticeshipLevel { get; set; }
           public string Category { get; set; }
           public string CategoryCode { get; set; }
           public DateTimeOffset ClosingDate { get; set; }
           public string Description { get; set; }
           public string EmployerName { get; set; }
           public string FrameworkLarsCode { get; set; }
           public long HoursPerWeek { get; set; }
           public bool IsDisabilityConfident { get; set; }
           public bool IsPositiveAboutDisability { get; set; }
           public bool IsRecruitVacancy { get; set; }
           public long NumberOfPositions { get; set; }
           public DateTimeOffset PostedDate { get; set; }
           public string ProviderName { get; set; }
           public int StandardLarsCode { get; set; }
           public DateTimeOffset StartDate { get; set; }
           public string SubCategory { get; set; }
           public string SubCategoryCode { get; set; }
           public string Title { get; set; }
           public int Ukprn { get; set; }
           public string VacancyLocationType { get; set; }
           public long VacancyReference { get; set; }
           public string WageAmount { get; set; }
           public string WageAmountLowerBound { get; set; }
           public string WageAmountUpperBound { get; set; }
           public string WageText { get; set; }
           public long WageUnit { get; set; }
           public long WageType { get; set; }
           public string WorkingWeek { get; set; }
           public long Score { get; set; }

           public static implicit operator GetVacanciesListResponseItem(GetVacanciesItem source)
           {
               return new GetVacanciesListResponseItem()
               {
                   Id = source.Id,
                   ApprenticeshipLevel = source.ApprenticeshipLevel,
                   Category = source.Category,
                   CategoryCode = source.CategoryCode,
                   ClosingDate = source.ClosingDate,
                   Description = source.Description,
                   EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                   FrameworkLarsCode = source.FrameworkLarsCode,
                   HoursPerWeek = source.HoursPerWeek,
                   IsDisabilityConfident = source.IsDisabilityConfident,
                   IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                   IsRecruitVacancy = source.IsRecruitVacancy,
                   NumberOfPositions = source.NumberOfPositions,
                   PostedDate = source.PostedDate,
                   ProviderName = source.ProviderName,
                   StandardLarsCode = source.StandardLarsCode,
                   StartDate = source.StartDate,
                   SubCategory = source.SubCategory,
                   SubCategoryCode = source.SubCategoryCode,
                   Title = source.Title,
                   Ukprn = source.Ukprn,
                   VacancyLocationType = source.VacancyLocationType,
                   VacancyReference = source.VacancyReference,
                   WageAmount = source.WageAmount,
                   WageAmountLowerBound = source.WageAmountLowerBound,
                   WageAmountUpperBound = source.WageAmountUpperBound,
                   WageText = source.WageText,
                   WageUnit =source.WageUnit,
                   WageType = source.WageType,
                   WorkingWeek = source.WorkingWeek,
                   Score = source.Score
                };
           }
        }
    }
}
