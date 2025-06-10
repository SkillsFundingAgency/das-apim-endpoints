using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;

public class GetAddressesQueryResult
{
    public IEnumerable<AddressItem> Addresses { get; set; } = Enumerable.Empty<AddressItem>();
}