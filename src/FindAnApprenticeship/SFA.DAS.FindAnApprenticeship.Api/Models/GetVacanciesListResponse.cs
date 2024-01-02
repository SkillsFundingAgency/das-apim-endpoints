using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetVacanciesListResponse
    {
        public IEnumerable<GetVacanciesListResponseItem> Vacancies { get; set; }

        public static implicit operator GetVacanciesListResponse(SearchApprenticeshipsResult source)
        {
            return new GetVacanciesListResponse()
            {
                Vacancies = source.Vacancies.Select(c =>(GetVacanciesListResponseItem)c)
            };
        }

    }
    public class GetVacanciesListResponseItem
    {
        public string Id { get; set; }
        public DateTime ClosingDate { get; set; }
        public string EmployerName { get; set; }
        public DateTime PostedDate { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string CourseTitle { get; set; }
        public string WageAmount { get; set; }
        public int WageType { get; set; }
        public string AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string AddressLine3 { get; private set; }
        public string? AddressLine4 { get; private set; }
        public string PostCode { get; private set; }
        public decimal? Distance { get; set; }
        public int CourseLevel { get; set; }
        public int CourseId { get; set; }
        public string CourseRoute { get; set; }

       public string ApprenticeshipLevel { get ; set ; }
       public string WageText { get; set; }

       public static implicit operator GetVacanciesListResponseItem(GetVacanciesListItem source)
       {
           return new GetVacanciesListResponseItem
           {
               Id = source.Id,
               ClosingDate = source.ClosingDate,
               EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
               PostedDate = source.PostedDate,
               Title = source.Title,
               VacancyReference = source.VacancyReference,
               Distance = source.Distance,
               ApprenticeshipLevel = source.ApprenticeshipLevel,
               CourseTitle = source.CourseTitle,
               CourseId = source.CourseId,
               WageType = source.WageType,
               WageAmount = source.WageAmount,
               WageText = source.WageText,
               AddressLine1 = source.Address.AddressLine1,
               AddressLine2 = source.Address.AddressLine2,
               AddressLine3 = source.Address.AddressLine3,
               AddressLine4 = source.Address.AddressLine4,
               PostCode = source.Address.Postcode,
               CourseRoute = source.CourseRoute,
               CourseLevel = source.CourseLevel
           };
       }
    }
}
