using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class SearchApprenticeshipsApiResponse
    {
        public static implicit operator SearchApprenticeshipsApiResponse(SearchApprenticeshipsResult source)
        {
            return new SearchApprenticeshipsApiResponse
            {
                TotalApprenticeshipCount = source.TotalApprenticeshipCount,
                Location = source.LocationItem,
                Routes = source.Routes.Select(c=>(RouteApiResponse)c).ToList(),
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItem)c).ToList(),
            };
        }
        
        [JsonPropertyName("totalApprenticeshipCount")]
        public long TotalApprenticeshipCount { get; set; }
        
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; set; }
        
        public List<RouteApiResponse> Routes { get; set; }
        public List<GetVacanciesListResponseItem> Vacancies { get; set; }

    }
}