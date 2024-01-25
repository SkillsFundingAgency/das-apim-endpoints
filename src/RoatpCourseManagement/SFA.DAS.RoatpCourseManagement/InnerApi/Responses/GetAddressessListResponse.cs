using System.Collections.Generic;
using System.Linq;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
public class GetAddressesListResponse
{
    public IEnumerable<GetAddressesListItem> Addresses { get; set; } = Enumerable.Empty<GetAddressesListItem>();
}
