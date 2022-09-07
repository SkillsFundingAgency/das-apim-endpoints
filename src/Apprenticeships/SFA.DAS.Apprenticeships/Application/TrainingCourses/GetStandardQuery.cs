using MediatR;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.Application.TrainingCourses;

public class GetStandardQuery : IRequest<GetStandardsListItem>
{
    public string CourseCode { get; set; }

    public GetStandardQuery(string courseCode)
    {
        CourseCode = courseCode;
    }
}

public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardsListItem>
{
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

    public GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    {
        _coursesApiClient = coursesApiClient;
    }

    public async Task<GetStandardsListItem> Handle(GetStandardQuery request, CancellationToken cancellationToken)
    {
        var course = await _coursesApiClient.Get<GetStandardsListItem>(
            new GetStandardDetailsByIdRequest(request.CourseCode));

        return course;
    }
}