using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

public class WhenBuildingGetCoursesRequest
{
    [Test]
    public void Then_The_Request_Url_Is_Active_Courses_Ordered_By_Score()
    {
        var request = new GetCoursesRequest();
        request.GetUrl.Should().Be($"api/courses/search?filter=Active&orderby={CoursesOrderBy.Score}");
    }
}