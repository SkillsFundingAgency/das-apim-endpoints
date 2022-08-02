using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses
{
    public class GetAllProviderCoursesQuery : IRequest<List<GetAllProviderCoursesQueryResult>>
    {
        public int Ukprn { get; }

        public GetAllProviderCoursesQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
