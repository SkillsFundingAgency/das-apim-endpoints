using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetStandards
{
    public class GetStandardsResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}
