using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;


namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public record SearchApprenticeshipsResult
    {
        public long TotalApprenticeshipCount { get; init; }
        public LocationItem LocationItem { get; set; }
        public List<GetRoutesListItem> Routes { get; set; }
        public IEnumerable<GetVacanciesListItem> Vacancies { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? VacancyReference { get; set; }
        public List<GetCourseLevelsListItem> Levels { get; set; }
        public long TotalFound { get; set; }
        public bool DisabilityConfident { get; set; }
        public int SavedSearchesCount { get; init; }
        public bool SearchAlreadySaved { get; init; }
    }
}