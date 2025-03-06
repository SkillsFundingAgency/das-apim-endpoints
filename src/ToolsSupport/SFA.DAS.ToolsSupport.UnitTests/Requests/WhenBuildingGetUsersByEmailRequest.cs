using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetUsersByEmailRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string email)
    {
        var actual = new GetUsersByEmailRequest(email);

        actual.GetUrl.Should().Be($"api/users?email={WebUtility.UrlEncode(email)}");
    }
}
