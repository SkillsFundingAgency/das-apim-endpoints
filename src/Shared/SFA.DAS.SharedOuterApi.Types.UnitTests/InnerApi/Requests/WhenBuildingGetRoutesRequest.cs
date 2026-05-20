using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetRoutesRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Created()
        {
            //Arrange
            var actual = new GetRoutesListRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/courses/routes");
        }
    }
}