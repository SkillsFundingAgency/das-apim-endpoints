using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetPayeSchemeLevyDeclarationsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string payeScheme)
    {
        var actual = new GetPayeSchemeLevyDeclarationsRequest(payeScheme);

        actual.GetUrl.Should().Be($"apprenticeship-levy/epaye/{HttpUtility.UrlEncode(payeScheme)}/declarations");
    }
}
