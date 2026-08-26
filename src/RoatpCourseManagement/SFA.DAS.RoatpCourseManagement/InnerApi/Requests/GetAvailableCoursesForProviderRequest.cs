using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAvailableCoursesForProviderRequest : IGetApiRequest
{
    private readonly int _ukprn;
    private readonly CourseType _courseType;
    public GetAvailableCoursesForProviderRequest(int ukprn, CourseType courseType)
    {
        _ukprn = ukprn;
        _courseType = courseType;
    }
    public string GetUrl => $"providers/{_ukprn}/course-types/{_courseType}/available-courses";
}
