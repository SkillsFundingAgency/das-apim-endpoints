using MediatR;
using SFA.DAS.Earnings.InnerApi;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Earnings.Application.Training;

public class GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetStandardQuery, GetStandardsListItem>
{
    public async Task<GetStandardsListItem> Handle(GetStandardQuery request, CancellationToken cancellationToken)
    {
        var course = await coursesApiClient.Get<GetStandardsListItem>(
            new GetStandardDetailsByIdRequest(request.CourseCode));

        return course;
    }
}