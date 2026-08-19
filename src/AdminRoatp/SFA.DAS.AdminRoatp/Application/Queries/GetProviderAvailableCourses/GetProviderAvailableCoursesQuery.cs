using MediatR;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAvailableCourses;

public class GetProviderAvailableCoursesQuery : IRequest<GetProviderAvailableCoursesQueryResult>
{
    public int Ukprn { get; set; }
    public CourseType? CourseType { get; set; }
}
