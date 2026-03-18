using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetAllCoursesRequest : IGetApiRequest
{
    public string GetUrl => $"standards?coursetype={CourseType}";
    public CourseType? CourseType { get; }
    public GetAllCoursesRequest(CourseType? courseType)
    {
        CourseType = courseType;
    }
}