using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses
{
    public class GetAllProviderCoursesQuery : IRequest<List<GetAllProviderCoursesQueryResult>>
    {
        public int Ukprn { get; }
        public CourseType? CourseType { get; }

        public GetAllProviderCoursesQuery(int ukprn, CourseType? courseType)
        {
            Ukprn = ukprn;
            CourseType = courseType;
        }
    }
}
