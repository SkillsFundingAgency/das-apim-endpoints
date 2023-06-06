using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAddressesListResponse
    {
        public IEnumerable<GetAddressesListItem> Addresses { get; set; } = Enumerable.Empty<GetAddressesListItem>();
    }
}
