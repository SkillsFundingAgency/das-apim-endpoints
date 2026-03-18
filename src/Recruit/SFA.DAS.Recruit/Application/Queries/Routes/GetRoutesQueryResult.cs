using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.Recruit.Application.Queries.Routes
{
    public class GetRoutesQueryResult
    {
        public IEnumerable<GetRoutesListItem> Routes { get ; set ; }
    }
}