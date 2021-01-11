using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetStandards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get ; set ; }
    }
}