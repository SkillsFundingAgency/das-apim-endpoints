using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;

public class GetAddressesListResponse
{
    public IEnumerable<GetAddressesListItem> Addresses { get; set; } = Enumerable.Empty<GetAddressesListItem>();
}