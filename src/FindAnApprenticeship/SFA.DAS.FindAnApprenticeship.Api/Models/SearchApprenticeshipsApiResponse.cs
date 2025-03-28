using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public record SearchApprenticeshipsApiResponse
    {
        public static implicit operator SearchApprenticeshipsApiResponse(SearchApprenticeshipsResult source)
        {
            return new SearchApprenticeshipsApiResponse
            {
                TotalApprenticeshipCount = source.TotalApprenticeshipCount,
                TotalCompetitiveVacanciesCount = source.TotalWageTypeVacanciesCount,
                Location = source.LocationItem,
                Routes = source.Routes.Select(c=>(RouteApiResponse)c).ToList(),
                Vacancies = source.Vacancies.Select(c => (GetVacanciesListResponseItem)c).ToList(),
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                VacancyReference = source.VacancyReference,
                Levels = source.Levels.Select(l => (LevelApiResponse)l).ToList(),
                TotalFound = source.TotalFound,
                DisabilityConfident = source.DisabilityConfident,
                SavedSearchesCount = source.SavedSearchesCount,
                SearchAlreadySaved = source.SearchAlreadySaved,
                CandidateDateOfBirth = source.CandidateDateOfBirth,
            };
        }
        [JsonPropertyName("candidateDateOfBirth")]
        public DateTime? CandidateDateOfBirth { get; set; }
        [JsonPropertyName("totalFound")]
        public long TotalFound { get; set; }
        [JsonPropertyName("totalApprenticeshipCount")]
        public long TotalApprenticeshipCount { get; init; }
        [JsonPropertyName("totalCompetitiveVacanciesCount")]
        public long TotalCompetitiveVacanciesCount { get; init; }
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; init; }
        public List<RouteApiResponse> Routes { get; init; }
        public List<LevelApiResponse> Levels { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public string? VacancyReference { get; init; }
        public List<GetVacanciesListResponseItem> Vacancies { get; init; }
        public bool DisabilityConfident { get; set; }
        public int SavedSearchesCount { get; init; }
        public bool SearchAlreadySaved { get; init; }
    }
}