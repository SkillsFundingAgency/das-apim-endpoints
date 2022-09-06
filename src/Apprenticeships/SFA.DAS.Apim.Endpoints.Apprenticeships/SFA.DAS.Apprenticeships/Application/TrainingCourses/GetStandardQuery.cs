using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Apprenticeships.Application.TrainingCourses
{
    //public class GetStandardQuery : IRequest<GetStandardsListItem> //todo commented out to allow deployment work to start
    //{
    //    public string CourseCode { get; set; }

    //    public GetStandardQuery(string courseCode)
    //    {
    //        CourseCode = courseCode;
    //    }
    //}

    //public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardsListItem>
    //{
    //    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

    //    public GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    //    {
    //        _coursesApiClient = coursesApiClient;
    //    }

    //    public async Task<GetStandardsListItem> Handle(GetStandardQuery request, CancellationToken cancellationToken)
    //    {
    //        var course = await _coursesApiClient.Get<GetStandardsListItem>(
    //            new GetStandardDetailsByIdRequest(request.CourseCode));

    //        return course;
    //    }
    //}
}
