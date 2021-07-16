using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Standards
{
    public class GetStandardsQueryResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}