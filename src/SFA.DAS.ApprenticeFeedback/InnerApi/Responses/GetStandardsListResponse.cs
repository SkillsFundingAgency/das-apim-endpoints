using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}
