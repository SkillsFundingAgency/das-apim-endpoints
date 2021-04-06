using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetNotYetApprovedStandardsRequest
    {
        [Test]
        public void Then_the_URL_is_correctly_built()
        {
            var actual = new GetNotYetApprovedStandardsRequest();

            actual.GetUrl.Should().Be("api/courses/standards?filter=NotYetApproved");
        }
    }
}
