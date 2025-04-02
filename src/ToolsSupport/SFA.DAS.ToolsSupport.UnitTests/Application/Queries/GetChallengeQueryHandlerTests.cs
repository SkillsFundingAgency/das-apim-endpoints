using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class GetChallengeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnChallengeResult_WhenAllDataIsValid(
        GetChallengeQuery query,
        Account account,
        GetChallengeQueryResult challengeResponse,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        [Frozen] Mock<IChallengeService> mockChallengeService,
        GetChallengeQueryHandler handler)
    {
        // Arrange
        var financeData = new GetAccountFinanceQueryResult();
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync(account);
        mockFinanceDataService.Setup(s => s.GetFinanceData(account)).ReturnsAsync(financeData);
        mockChallengeService.Setup(s => s.GetChallengeQueryResultFromAccount(account, financeData.PayeSchemes))
            .Returns(challengeResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(challengeResponse);
        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockFinanceDataService.Verify(s => s.GetFinanceData(account), Times.Once());
        mockChallengeService.Verify(s => s.GetChallengeQueryResultFromAccount(account, financeData.PayeSchemes), Times.Once());
    }
}
