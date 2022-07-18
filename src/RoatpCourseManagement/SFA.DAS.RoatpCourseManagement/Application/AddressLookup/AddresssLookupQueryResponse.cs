using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup
{
    public class AddresssLookupQueryResponse
    {
        public IEnumerable<AddressItem> Addresses { get; set; }
    }
}
