using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllCoursesQuery
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
