using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAllProviderCoursesRequest : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/courses?excludeCoursesWithoutLocation={ExcludeInvalidCourses}&courseType={CourseType}";
    public int Ukprn { get; }
    public bool ExcludeInvalidCourses { get; }
    public CourseType CourseType { get; }

    public GetAllProviderCoursesRequest(int ukprn, CourseType courseType)
    {
        Ukprn = ukprn;
        ExcludeInvalidCourses = false;
        CourseType = courseType;
    }
}