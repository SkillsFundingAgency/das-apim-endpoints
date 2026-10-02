using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using System;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetAccountTransactionSummaryByDateRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_Request_With_Correct_AccountId(long accountId, DateTime fromDate, DateTime toDate)
    {
        // Act
        var request = new GetAccountTransactionSummaryByDateRequest(accountId, fromDate, toDate);

        // Assert
        request.AccountId.Should().Be(accountId);
        request.FromDate.Should().Be(fromDate);
        request.ToDate.Should().Be(toDate);
        request.GetUrl.Should().Be($"api/accounts/{accountId}/transaction-summary?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");
    }
}
