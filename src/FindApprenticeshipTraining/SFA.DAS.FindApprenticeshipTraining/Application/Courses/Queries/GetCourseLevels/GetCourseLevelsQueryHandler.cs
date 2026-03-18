using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;

public sealed class GetCourseLevelsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient) : IRequestHandler<GetCourseLevelsQuery, GetCourseLevelsListResponse>
{
    public async Task<GetCourseLevelsListResponse> Handle(GetCourseLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = await _coursesApiClient.Get<GetCourseLevelsListResponse>(new GetCourseLevelsListRequest());
        return result;
    }
}
