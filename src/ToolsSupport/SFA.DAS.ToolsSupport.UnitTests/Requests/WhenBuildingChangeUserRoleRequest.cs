using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingChangeUserRoleRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(ChangeUserRoleRequestData data)
    {
        var actual = new ChangeUserRoleRequest(data);

        actual.PostUrl.Should().Be("api/support/change-role");
        actual.Data.Should().NotBeNull();
        actual.Data.Should().BeEquivalentTo(data);
    }
}
