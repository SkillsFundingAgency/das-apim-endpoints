using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}