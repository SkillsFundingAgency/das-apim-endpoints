using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
public class GetAllShortCourseTypesQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetAllShortCourseTypesQueryHandler> _logger) : IRequestHandler<GetAllShortCourseTypesQuery, GetAllCourseTypesResponse>
{
    public async Task<GetAllCourseTypesResponse> Handle(GetAllShortCourseTypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle get all short course types request");

        var response = await _apiClient.GetWithResponseCode<GetAllCourseTypesResponse>(new GetAllCourseTypesRequest());

        response.EnsureSuccessStatusCode();

        return new() { CourseTypes = response.Body.CourseTypes.Where(r => r.LearningType == LearningType.ShortCourse).ToList() };
    }
}