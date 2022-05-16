using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandards
{
    public class GetAllStandardsQuery : IRequest<ApiResponse<GetAllStandardsResponse>>
    {
    }
}
