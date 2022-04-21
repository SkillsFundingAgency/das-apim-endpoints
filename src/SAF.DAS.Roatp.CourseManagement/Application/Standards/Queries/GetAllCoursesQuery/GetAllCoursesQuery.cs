using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.Queries.GetCourseQuery
{
    public class GetAllCoursesQuery : IRequest<List<GetAllCoursesResult>>
    {
        public int Ukprn { get; }

        public GetAllCoursesQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
