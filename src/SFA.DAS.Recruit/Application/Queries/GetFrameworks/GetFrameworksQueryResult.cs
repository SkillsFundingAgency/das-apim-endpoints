using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetFrameworks
{
    public class GetFrameworksQueryResult
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get ; set ; }
    }
}