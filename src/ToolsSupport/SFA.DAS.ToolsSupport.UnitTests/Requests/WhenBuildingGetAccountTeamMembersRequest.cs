using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetAccountTeamMembersRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(long id)
    {
        var actual = new GetAccountTeamMembersRequest(id);

        actual.GetUrl.Should().Be($"api/accounts/internal/{id}/users");
    }
}
