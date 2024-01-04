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
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                VacancyReference = source.VacancyReference,
                TotalFound = source.TotalFound
            };
        }
        [JsonPropertyName("totalFound")]
        public long TotalFound { get; set; }

        [JsonPropertyName("totalApprenticeshipCount")]
        public long TotalApprenticeshipCount { get; init; }
        
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; init; }
        
        public List<RouteApiResponse> Routes { get; set; }
        public List<GetVacanciesListResponseItem> Vacancies { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? VacancyReference { get; set; }
    }
}