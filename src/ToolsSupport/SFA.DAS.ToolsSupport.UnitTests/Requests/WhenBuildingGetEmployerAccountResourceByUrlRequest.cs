using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetEmployerAccountResourceByUrlRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string url)
    {
        var actual = new GetEmployerAccountResourceByUrlRequest(url);

        actual.GetUrl.Should().Be(url);
    }
}
