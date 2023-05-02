using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.InnerApi.Regions;

public class GetRegionsQueryRequestTests
{
    [Test]
    public void CheckRequestUrl()
    {
        var request = new GetRegionsQueryRequest();
        request.GetUrl.Should().Be("/regions");
    }
}