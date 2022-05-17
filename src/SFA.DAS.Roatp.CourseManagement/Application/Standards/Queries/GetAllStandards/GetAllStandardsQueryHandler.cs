using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandards
{
    public class GetAllStandardsQueryHandler : IRequestHandler<GetAllStandardsQuery, ApiResponse<GetAllStandardsResponse>>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<GetAllStandardsQueryHandler> _logger;

        public GetAllStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILogger<GetAllStandardsQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _logger = logger;
        }

        public async Task<ApiResponse<GetAllStandardsResponse>> Handle(GetAllStandardsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all standards request received");
            return await _coursesApiClient.GetWithResponseCode<GetAllStandardsResponse>(new GetAllStandardsRequest());
        }
    }
}
