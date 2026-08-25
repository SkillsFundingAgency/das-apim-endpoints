using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAvailableCoursesForProviderRequest : IGetApiRequest
{
    private int _ukprn;
    private CourseType _courseType;
    public GetAvailableCoursesForProviderRequest(int ukprn, CourseType courseType)
    {
        _ukprn = ukprn;
        _courseType = courseType;
    }
    public string GetUrl => $"providers/{_ukprn}/course-types/{_courseType}/available-courses";
}
