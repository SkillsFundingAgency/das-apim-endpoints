using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;

public sealed class GetCourseRoutesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient) : IRequestHandler<GetCourseRoutesQuery, GetRoutesListResponse>
{
    public async Task<GetRoutesListResponse> Handle(GetCourseRoutesQuery request, CancellationToken cancellationToken)
    {
        return await _coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());
    }
}
