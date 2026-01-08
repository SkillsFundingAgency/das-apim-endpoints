using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAllCoursesRequest : IGetApiRequest
{
    public string GetUrl => BuildUrl();
    public int Ukprn { get; }
    public bool ExcludeInvalidCourses { get; }
    public CourseType? CourseType { get; }
    private string BuildUrl()
    {
        var url = $"providers/{Ukprn}/courses?excludeCoursesWithoutLocation={ExcludeInvalidCourses}";

        if (CourseType.HasValue)
        {
            url += $"&courseType={CourseType}";
        }

        return url;
    }

    public GetAllCoursesRequest(int ukprn, CourseType? courseType)
    {
        Ukprn = ukprn;
        ExcludeInvalidCourses = false;
        CourseType = courseType;
    }
}