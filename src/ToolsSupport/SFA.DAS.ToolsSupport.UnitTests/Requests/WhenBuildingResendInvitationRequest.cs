using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingResendInvitationRequest
{

    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(ResendInvitationRequestData data)
    {
        var actual = new ResendInvitationRequest(data);

        actual.PostUrl.Should().Be("api/support/resend-invitation");
        actual.Data.Should().NotBeNull();
        actual.Data.Should().BeEquivalentTo(data);
    }
}
