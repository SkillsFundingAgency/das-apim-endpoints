using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsResult
    {
        public long TotalApprenticeshipCount { get; set; }
        public LocationItem LocationItem { get; set; }
        public List<GetRoutesListItem> Routes { get; set; }
    }
}