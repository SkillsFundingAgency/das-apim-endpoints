using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetAllCoursesQueryHandler> _logger) : IRequestHandler<GetAllCoursesQuery, GetAllCoursesQueryResult>
{
    public async Task<GetAllCoursesQueryResult> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllCourses request");

        var response = await _courseManagementApiClient.GetWithResponseCode<GetAllStandardsResponse>(new GetStandardsRequest());

        response.EnsureSuccessStatusCode();

        return new GetAllCoursesQueryResult
        {
            Courses = response.Body.Standards
                .Select(standard => new CourseModel
                {
                    LarsCode = standard.LarsCode,
                    CourseType = standard.CourseType,
                    LearningType = standard.LearningType,
                    Title = standard.Title,
                    Level = standard.Level
                })
                .ToList()
        };
    }
}
