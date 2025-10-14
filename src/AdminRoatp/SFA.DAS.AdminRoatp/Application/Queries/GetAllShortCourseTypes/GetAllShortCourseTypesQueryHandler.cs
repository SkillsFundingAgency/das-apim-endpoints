using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
public class GetAllShortCourseTypesQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetAllShortCourseTypesQuery, GetAllShortCourseTypesQueryResult>
{
    public async Task<GetAllShortCourseTypesQueryResult> Handle(GetAllShortCourseTypesQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<List<CourseTypeSummary>>(new GetAllCourseTypesRequest());

        return new() { CourseTypes = response.Body.Where(r => r.LearningType == LearningType.ShortCourse).ToList() };
    }
}
