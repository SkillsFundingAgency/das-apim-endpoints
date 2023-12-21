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
        public List<GetRoutesListItem> Routes { get; init; }
        public IEnumerable<GetVacanciesListItem> Vacancies { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }
}