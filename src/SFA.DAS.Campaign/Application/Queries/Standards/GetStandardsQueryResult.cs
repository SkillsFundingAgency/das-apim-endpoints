using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Application.Queries.Standards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}