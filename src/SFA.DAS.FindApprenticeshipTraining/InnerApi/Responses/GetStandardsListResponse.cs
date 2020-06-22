using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}
