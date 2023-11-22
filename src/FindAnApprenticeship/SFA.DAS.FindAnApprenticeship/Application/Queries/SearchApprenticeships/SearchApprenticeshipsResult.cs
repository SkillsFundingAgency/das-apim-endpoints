using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Api.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsResult
    {
        public long TotalApprenticeshipCount { get; set; }
        public LocationItem LocationItem { get; set; }
        public List<GetRoutesListItem> Routes { get; set; }
        public List<GetVacanciesListResponse> Vacancies { get; set; }
    }
}