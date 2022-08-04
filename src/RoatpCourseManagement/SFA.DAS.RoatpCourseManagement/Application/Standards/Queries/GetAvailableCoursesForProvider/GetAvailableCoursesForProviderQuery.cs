using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider
{
    public class GetAvailableCoursesForProviderQuery : IRequest<GetAvailableCoursesForProviderQueryResult>
    {
        public int Ukprn { get; }
        public GetAvailableCoursesForProviderQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
