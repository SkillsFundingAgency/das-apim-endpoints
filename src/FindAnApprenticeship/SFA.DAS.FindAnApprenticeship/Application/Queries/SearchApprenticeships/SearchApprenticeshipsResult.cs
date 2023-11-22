using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;
using GetRoutesListItem = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetRoutesListItem;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsResult
    {
        public long TotalApprenticeshipCount { get; set; }
        public LocationItem LocationItem { get; set; }
        public List<GetRoutesListItem> Routes { get; set; }
        public GetVacancyApiResponse Vacancies { get; set; }
    }
}