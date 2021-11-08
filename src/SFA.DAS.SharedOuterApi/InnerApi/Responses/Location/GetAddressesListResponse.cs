using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAddressesListResponse
    {
        public IEnumerable<GetAddressesListItem> Addresses { get; set; }
    }
}
