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
                TotalPages = source.TotalPages
            };
        }
        
        [JsonPropertyName("totalApprenticeshipCount")]
        public long TotalApprenticeshipCount { get; init; }
        
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; init; }
        
        public List<RouteApiResponse> Routes { get; init; }
        public List<GetVacanciesListResponseItem> Vacancies { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }
}