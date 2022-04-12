using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.Routes
{
    public class GetRoutesQueryResult
    {
        public IEnumerable<GetRoutesListItem> Routes { get ; set ; }
    }
}