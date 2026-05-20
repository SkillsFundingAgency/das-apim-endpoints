using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsClosedToNewStarts
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            //Act
            var actual = new GetStandardsClosedToNewStartsRequest();

            //Assert
            Assert.That("api/courses/standards?filter=ClosedToNewStarts", Is.EqualTo(actual.GetUrl));
        }
    }
}