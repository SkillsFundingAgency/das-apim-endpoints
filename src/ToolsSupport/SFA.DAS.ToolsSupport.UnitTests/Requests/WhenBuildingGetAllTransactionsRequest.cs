using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetAllTransactionsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string accountId, DateTime fromdate, DateTime toDate)
    {
        var actual = new GetAllTransactionsRequest(accountId, fromdate, toDate);

        actual.GetUrl.Should().Be($"api/accounts/{accountId}/transactions/query?fromDate={fromdate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");
    }
}
