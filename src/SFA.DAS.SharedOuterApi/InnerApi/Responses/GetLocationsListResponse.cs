using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetLocationsListResponse
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }
}
