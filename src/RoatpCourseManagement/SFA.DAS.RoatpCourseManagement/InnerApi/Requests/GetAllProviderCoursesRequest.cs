using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAllProviderCoursesRequest : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/courses?excludeCoursesWithoutLocation={ExcludeInvalidCourses}&courseType={CourseType}";
    public int Ukprn { get; }
    public bool ExcludeInvalidCourses { get; }
    public CourseType? CourseType { get; }

    public GetAllProviderCoursesRequest(int ukprn, CourseType? courseType)
    {
        Ukprn = ukprn;
        ExcludeInvalidCourses = false;
        CourseType = courseType;
    }
}