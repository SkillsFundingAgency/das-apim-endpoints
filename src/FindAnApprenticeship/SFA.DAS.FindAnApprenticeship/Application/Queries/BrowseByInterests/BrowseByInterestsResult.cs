using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests
{
    public class BrowseByInterestsResult
    {
        public List<GetRoutesListItem> Routes { get; set; }
    }
}