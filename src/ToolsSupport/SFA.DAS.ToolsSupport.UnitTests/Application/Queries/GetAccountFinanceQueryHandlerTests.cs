using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class GetAccountFinanceQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnFinanceData_WhenAccountExists(
        GetAccountFinanceQuery query,
        Account account,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        GetAccountFinanceQueryHandler handler)
    {
        // Arrange
        var financeData = new GetAccountFinanceQueryResult();
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync(account).Verifiable();
        mockFinanceDataService.Setup(s => s.GetFinanceData(account)).ReturnsAsync(financeData).Verifiable();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(financeData);

        mockFinanceDataService.Verify();
        mockFinanceDataService.VerifyNoOtherCalls();
        mockAccountsService.Verify();
        mockAccountsService.VerifyNoOtherCalls();
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnEmptyResult_WhenAccountIsNull(
        GetAccountFinanceQuery query,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        GetAccountFinanceQueryHandler handler)
    {
        // Arrange
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync((Account)null)
            .Verifiable();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new GetAccountFinanceQueryResult());
        mockAccountsService.Verify();
        mockAccountsService.VerifyNoOtherCalls();

        mockFinanceDataService.Verify(s => s.GetFinanceData(It.IsAny<Account>()), Times.Never());
    }
}
