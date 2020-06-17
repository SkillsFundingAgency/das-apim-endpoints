using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}
