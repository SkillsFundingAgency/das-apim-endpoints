using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider
{
    public class GetAvailableCoursesForProviderQuery : IRequest<GetAvailableCoursesForProviderQueryResult>
    {
        public int Ukprn { get; }
        public CourseType? CourseType { get; }
        public GetAvailableCoursesForProviderQuery(int ukprn, CourseType? courseType)
        {
            Ukprn = ukprn;
            CourseType = courseType;
        }
    }
}
