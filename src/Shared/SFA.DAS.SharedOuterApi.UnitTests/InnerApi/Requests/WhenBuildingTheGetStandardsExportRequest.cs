using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsExportRequest
    {
        [Test]
        public void Then_the_URL_is_correctly_built()
        {
            var actual = new GetStandardsExportRequest();

            actual.GetUrl.Should().Be("ops/export");
        }
    }
}
