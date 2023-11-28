﻿using System;
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
        public long Id { get; set; }
        public DateTime ClosingDate { get; set; }
        public string EmployerName { get; set; }
        public DateTime PostedDate { get; set; }
        public string Title { get; set; }
        public string VacancyReference { get; set; }
        public string CourseTitle { get; set; }
        public double? WageAmount { get; set; }
        public string WageType { get; set; }
        public string AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string AddressLine3 { get; private set; }
        public string? AddressLine4 { get; private set; }
        public string PostCode { get; private set; }
        public decimal? Distance { get; set; }
        public int CourseLevel { get; set; }
        public string Route { get; set; }

       public string ApprenticeshipLevel { get ; set ; }

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
               CourseTitle = source.Course.Title,
               CourseLevel = source.Course.Level,
               Route = source.Course.Route,
               WageType = source.Wage.WageType,
               WageAmount = source.Wage.WageAmount,
               AddressLine1 = source.Address.AddressLine1,
               AddressLine2 = source.Address.AddressLine2,
               AddressLine3 = source.Address.AddressLine3,
               AddressLine4 = source.Address.AddressLine4,
               PostCode = source.Address.Postcode
           };
       }
    }
}
