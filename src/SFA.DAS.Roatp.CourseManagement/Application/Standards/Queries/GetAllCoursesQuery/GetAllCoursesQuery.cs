using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery
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
