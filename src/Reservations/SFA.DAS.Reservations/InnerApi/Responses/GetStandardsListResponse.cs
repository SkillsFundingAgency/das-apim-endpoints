using System.Collections.Generic;

namespace SFA.DAS.Reservations.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}