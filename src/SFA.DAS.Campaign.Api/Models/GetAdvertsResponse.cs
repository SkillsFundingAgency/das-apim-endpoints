using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Application.Queries.Adverts;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetAdvertsResponse
    {
        public IEnumerable<GetRouteResponseItem> Routes { get; set; }
        public long TotalFound { get; set; }
        public GetLocationResponseItem Location { get; set; }
        public IEnumerable<GetAdvertResponseItem> Vacancies { get; set; }

        public static implicit operator GetAdvertsResponse(GetAdvertsQueryResult source)
        {
            return new GetAdvertsResponse
            {
                TotalFound = source.TotalFound,
                Routes = source.Routes.Select(c => (GetRouteResponseItem)c),
                Location = source.Location,
                Vacancies = source.Vacancies.Select(c => (GetAdvertResponseItem)c)
            };
        }
    }

    public class GetLocationResponseItem
    {
        public double[] GeoPoint { get ; set ; }
        public string Name { get ; set ; }
        public string Country { get ; set ; }

        public static implicit operator GetLocationResponseItem(LocationItem source)
        {
            if (source == null)
            {
                return null;
            }
            return new GetLocationResponseItem
            {
                Country = source.Country,
                Name = source.Name,
                GeoPoint = source.GeoPoint
            };
        }
    }

    public class GetAdvertResponseItem
    {
        public VacancyLocation Location { get ; set ; }
        public DateTime StartDate { get ; set ; }
        public DateTime ClosingDate { get ; set ; }
        public string Description { get ; set ; }
        public string Title { get ; set ; }
        public decimal? Distance { get ; set ; }
        public DateTime PostedDate { get ; set ; }
        public string Category { get ; set ; }
        public string EmployerName { get ; set ; }
        public string VacancyReference { get ; set ; }
        public string VacancyUrl { get ; set ; }
        public string SubCategory { get ; set ; }


        public static implicit operator GetAdvertResponseItem(GetVacanciesListItem source)
        {
            return new GetAdvertResponseItem
            {
                Distance = source.Distance,
                Title = source.Title,
                Description = source.Description,
                ClosingDate = source.ClosingDate,
                StartDate = source.StartDate,
                PostedDate = source.PostedDate,
                Category = source.Category,
                SubCategory = source.SubCategory,
                Location = new VacancyLocation
                {
                    Lat = source.Location.Lat,
                    Lon = source.Location.Lon
                },
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                VacancyReference = source.VacancyReference,
                VacancyUrl = source.VacancyUrl
            };
        }
    }
    
    public class VacancyLocation
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
}