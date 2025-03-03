
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
