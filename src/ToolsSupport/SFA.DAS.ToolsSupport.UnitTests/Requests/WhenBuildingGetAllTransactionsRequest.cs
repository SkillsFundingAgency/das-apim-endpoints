using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetAllTransactionsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string accountId, int year, int month)
    {
        var actual = new GetAllTransactionsRequest(accountId, year, month);

        actual.GetUrl.Should().Be($"api/accounts/{accountId}/transactions/all-transactions/{year}/{month}");
    }
}
