using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetAccountBalancesRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(List<string> accountIds)
    {
        var actual = new GetAccountBalancesRequest(accountIds);

        actual.PostUrl.Should().Be("api/accounts/balances");
        actual.Data.Should().NotBeNull();
        actual.Data.Should().BeEquivalentTo(accountIds);
    }
}
