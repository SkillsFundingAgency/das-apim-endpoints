using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingTheGetRedirectsRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            var actual = new GetRedirectsRequest();

            actual.GetUrl.Should().Be($"entries?content_type=redirect&limit={GetRedirectsRequest.MaxRedirects}");
        }
    }
}
