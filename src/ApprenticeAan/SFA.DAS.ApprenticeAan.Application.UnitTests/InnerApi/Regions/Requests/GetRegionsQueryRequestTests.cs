using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions.Requests;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Regions.Requests
{
    public class GetRegionsQueryRequestTests
    {
        [Test]
        public void CheckRequestUrl()
        {
            var request = new GetRegionsQueryRequest();
            request.GetUrl.Should().Be("/api/regions");
        }
    }
}