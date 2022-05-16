using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAllFrameworksRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            var actual = new GetAllFrameworksRequest();

            actual.GetUrl.Should().Be("api/courses/frameworks");
        }
    }
}