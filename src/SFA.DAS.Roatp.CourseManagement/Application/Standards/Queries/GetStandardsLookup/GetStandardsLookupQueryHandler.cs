using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardsLookup
{
    public class GetStandardsLookupQueryHandler : IRequestHandler<GetStandardsLookupQuery, ApiResponse<GetStandardsLookupResponse>>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<GetStandardsLookupQueryHandler> _logger;

        public GetStandardsLookupQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILogger<GetStandardsLookupQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _logger = logger;
        }

        public async Task<ApiResponse<GetStandardsLookupResponse>> Handle(GetStandardsLookupQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all standards request received");
            return await _coursesApiClient.GetWithResponseCode<GetStandardsLookupResponse>(new GetStandardsLookupRequest());
        }
    }
}
