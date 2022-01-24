using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.OpenApi.Extensions;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacanciesListResponse 
    {
        public List<GetVacanciesListResponseItem> Vacancies { get; set; }
        public long Total { get ; set ; }
        public long TotalFiltered { get ; set ; }
        public int TotalPages { get ; set ; }

        public static implicit operator GetVacanciesListResponse(GetVacanciesQueryResult source)
        {
            return new GetVacanciesListResponse()
            {
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItem)c).ToList(),
                Total = source.Total,
                TotalFiltered = source.TotalFiltered,
                TotalPages = source.TotalPages
            };
        }

        public class GetVacanciesListResponseItem
       {
           public DateTimeOffset ClosingDate { get; set; }    
           public string Description { get; set; }
           public string EmployerName { get; set; }
           public decimal HoursPerWeek { get; set; }
           public bool IsDisabilityConfident { get; set; }
           public  bool IsNationalVacancy { get; set; }
           public long NumberOfPositions { get; set; }
           public DateTimeOffset PostedDate { get; set; }
           public string ProviderName { get; set; }
           public DateTimeOffset StartDate { get; set; }
           public string Title { get; set; }
           public int Ukprn { get; set; }
           public long VacancyReference { get; set; }
           public string VacancyUrl { get; set; }

           public VacancyCourseItem Course { get; set; }
           public static implicit operator GetVacanciesListResponseItem(GetVacanciesListItem source)
           {
               return new GetVacanciesListResponseItem
               {
                   ClosingDate = source.ClosingDate,
                   Description = source.Description,
                   EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                   HoursPerWeek = source.HoursPerWeek,
                   IsDisabilityConfident = source.IsDisabilityConfident,
                   IsNationalVacancy = source.VacancyLocationType.Equals("National", StringComparison.CurrentCultureIgnoreCase),
                   NumberOfPositions = source.NumberOfPositions,
                   PostedDate = source.PostedDate,
                   ProviderName = source.ProviderName,
                   StartDate = source.StartDate,
                   Title = source.Title,
                   Ukprn = source.Ukprn,
                   VacancyReference = source.VacancyReference,
                   VacancyUrl = source.VacancyUrl,
                   Course = new VacancyCourseItem
                   {
                       LarsCode = source.StandardLarsCode.Value,
                       Level = source.CourseLevel,
                       Route = source.Route,
                       Title = $"{source.CourseTitle} (level {source.CourseLevel})"
                   }
               };
           }
       }

        public class VacancyCourseItem
        {
            public int LarsCode { get; set; }
            public string Title { get; set; }
            public int Level { get; set; }
            public string Route { get; set; }
        }
        
    }
}
