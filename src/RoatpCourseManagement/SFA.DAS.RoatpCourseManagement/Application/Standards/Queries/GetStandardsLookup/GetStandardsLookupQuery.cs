using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardsLookup
{
    public class GetStandardsLookupQuery : IRequest<ApiResponse<GetStandardsLookupResponseFromCoursesApi>>
    {
    }
}
