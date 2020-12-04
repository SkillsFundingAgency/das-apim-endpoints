using System.Collections.Generic;

namespace SFA.DAS.EpaoRegister.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}