using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;

public class GetAddressesListResponse
{
    public IEnumerable<GetAddressesListItem> Addresses { get; set; } = Enumerable.Empty<GetAddressesListItem>();
}