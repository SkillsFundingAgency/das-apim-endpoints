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
    }
}