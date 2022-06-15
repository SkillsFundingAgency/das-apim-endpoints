using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardsLookup
{
    public class GetStandardsLookupQuery : IRequest<ApiResponse<GetStandardsLookupResponse>>
    {
    }
}
