using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingSendInvitationRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(SendInvitationRequestData data)
    {
        var actual = new SendInvitationRequest(data);

        actual.PostUrl.Should().Be("api/support/send-invitation");
        actual.Data.Should().NotBeNull();
        actual.Data.Should().BeEquivalentTo(data);
    }
}
