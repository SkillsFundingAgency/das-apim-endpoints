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

    }
    public class GetVacanciesListResponseItem
    {
       public DateTime ClosingDate { get; set; }    
       public string Description { get; set; }
       public string EmployerName { get; set; }
       public decimal HoursPerWeek { get; set; }
       public bool IsDisabilityConfident { get; set; }
       public  bool IsNationalVacancy { get; set; }
       public long NumberOfPositions { get; set; }
       public DateTime PostedDate { get; set; }
       public string ProviderName { get; set; }
       public DateTime StartDate { get; set; }
       public string Title { get; set; }
       public int Ukprn { get; set; }
       public string VacancyReference { get; set; }
       public string VacancyUrl { get; set; }

       public GetVacancyCourseItem Course { get; set; }
       public GetVacancyWageItem Wage { get; set; }
       public VacancyLocation Location { get; set; }
       public GetVacancyAddressItem Address { get; set; }
       public decimal? Distance { get ; set ; }

       public string EmployerContactPhone { get ; set ; }
       public string EmployerContactName { get ; set ; }
       public string EmployerContactEmail { get ; set ; }
       public string EmployerWebsiteUrl { get ; set ; }
       public string ApprenticeshipLevel { get ; set ; }
       public string ExpectedDuration { get ; set ; }

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
               Course = source,
               Wage = source,
               Distance = source.Distance,
               Address = source,
               EmployerWebsiteUrl = source.EmployerWebsiteUrl,
               EmployerContactEmail = source.EmployerContactEmail,
               EmployerContactName = source.EmployerContactName,
               EmployerContactPhone = source.EmployerContactPhone,
               ApprenticeshipLevel = source.ApprenticeshipLevel,
               ExpectedDuration = source.ExpectedDuration,
               Location = new VacancyLocation
               {
                   Lat = source.Location.Lat,
                   Lon = source.Location.Lon
               }
           };
       }
    }

    public class VacancyLocation
    {
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }

    public enum WageType
    {
        ApprenticeshipMinimum = 2,
        NationalMinimum = 3,
        Custom = 4,
        CompetitiveSalary = 6
    }
    public enum WageUnit
    {
        Unspecified = 1,
        Weekly = 2,
        Monthly = 3,
        Annually = 4
    }
}
