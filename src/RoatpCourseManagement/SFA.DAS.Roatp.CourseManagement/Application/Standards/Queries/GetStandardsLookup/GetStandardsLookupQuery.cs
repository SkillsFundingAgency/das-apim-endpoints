using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardsLookup
{
    public class GetStandardsLookupQuery : IRequest<ApiResponse<GetStandardsLookupResponse>>
    {
    }
}
