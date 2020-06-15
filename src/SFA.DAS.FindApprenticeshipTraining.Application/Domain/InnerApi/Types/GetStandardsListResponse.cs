using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Types
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
