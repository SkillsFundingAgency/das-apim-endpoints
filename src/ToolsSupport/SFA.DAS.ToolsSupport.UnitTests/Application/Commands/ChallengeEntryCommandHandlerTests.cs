using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
using SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;
using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Models;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Commands;
[TestFixture]
public class ChallengeEntryCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnValidResult_WhenDataCheckPasses(
        ChallengeEntryCommand command,
        Account account,
        GetChallengeQueryResult challengeResponse,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        [Frozen] Mock<IChallengeService> mockChallengeService,
        [Frozen] Mock<IEmployerFinanceService> mockFinanceService,
        ChallengeEntryCommandHandler handler)
    {
        // Arrange
        command.Challenge1 = "z";
        command.Challenge2 = "d";
        command.Balance = "1000";
        command.FirstCharacterPosition = 2;
        command.SecondCharacterPosition = 4;
        mockAccountsService.Setup(s => s.GetAccount(command.AccountId)).ReturnsAsync(account);

        var financeData = new GetAccountFinanceQueryResult();
        mockFinanceDataService.Setup(s => s.GetFinanceData(account)).ReturnsAsync(financeData);

        challengeResponse.StatusCode = SearchResponseCodes.Success;
        challengeResponse.Characters = [2, 4];
        mockChallengeService.Setup(s => s.GetChallengeQueryResultFromAccount(account, It.IsAny<IEnumerable<PayeScheme>>()))
            .Returns(challengeResponse);

        var balanceResponse = new ApiResponse<List<AccountBalance>>([new AccountBalance { Balance = 1000m }], HttpStatusCode.OK, "");


        mockFinanceService.Setup(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()))
            .ReturnsAsync(balanceResponse);

        financeData.PayeSchemes = [new PayeScheme { Ref = "xyzcd" }];

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.IsValid.Should().BeTrue();
        result.Characters.Should().BeEquivalentTo(challengeResponse.Characters);
        mockFinanceService.Verify(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnInvalidResult_WhenDataCheckFails(
        ChallengeEntryCommand command,
        Account account,
        GetChallengeQueryResult challengeResponse,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        [Frozen] Mock<IChallengeService> mockChallengeService,
        [Frozen] Mock<IEmployerFinanceService> mockFinanceService,
        ChallengeEntryCommandHandler handler)
    {
        // Arrange
        var financeData = new GetAccountFinanceQueryResult();

        command.Challenge1 = "a";
        command.Challenge2 = "b";
        command.Balance = "1000";
        command.FirstCharacterPosition = 2;
        command.SecondCharacterPosition = 4;
        mockAccountsService.Setup(s => s.GetAccount(command.AccountId)).ReturnsAsync(account);

        financeData.PayeSchemes = [new PayeScheme { Ref = "xyzcd" }];
        mockFinanceDataService.Setup(s => s.GetFinanceData(account)).ReturnsAsync(financeData);

        challengeResponse.StatusCode = SearchResponseCodes.Success;
        challengeResponse.Characters = [2, 4];
        mockChallengeService.Setup(s => s.GetChallengeQueryResultFromAccount(account, It.IsAny<IEnumerable<PayeScheme>>()))
            .Returns(challengeResponse);

        var balanceResponse = new ApiResponse<List<AccountBalance>>([new AccountBalance { Balance = 2000m }], HttpStatusCode.OK, "");
        mockFinanceService.Setup(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()))
            .ReturnsAsync(balanceResponse);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.IsValid.Should().BeFalse();
        result.Characters.Should().BeEquivalentTo(challengeResponse.Characters);
        mockFinanceService.Verify(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnInvalidResult_WhenInputIsInvalid(
        ChallengeEntryCommand command,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        [Frozen] Mock<IChallengeService> mockChallengeService,
        [Frozen] Mock<IEmployerFinanceService> mockFinanceService,
        ChallengeEntryCommandHandler handler)
    {
        // Arrange
        var financeData = new GetAccountFinanceQueryResult();

        command.Challenge1 = "";

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.IsValid.Should().BeFalse();
        result.Characters.Should().BeEquivalentTo([command.FirstCharacterPosition, command.SecondCharacterPosition]);
        mockFinanceService.Verify(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()), Times.Never());
        mockAccountsService.Verify(s => s.GetAccount(It.IsAny<long>()), Times.Never());
        mockFinanceDataService.Verify(s => s.GetFinanceData(It.IsAny<Account>()), Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnInvalidResult_WhenChallengeResponseStatusIsNotSuccess(
        ChallengeEntryCommand command,
        Account account,
        GetChallengeQueryResult challengeResponse,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IFinanceDataService> mockFinanceDataService,
        [Frozen] Mock<IChallengeService> mockChallengeService,
        [Frozen] Mock<IEmployerFinanceService> mockFinanceService,
        ChallengeEntryCommandHandler handler)
    {
        // Arrange
        var financeData = new GetAccountFinanceQueryResult();

        mockAccountsService.Setup(s => s.GetAccount(command.AccountId)).ReturnsAsync(account);
        mockFinanceDataService.Setup(s => s.GetFinanceData(account)).ReturnsAsync(financeData);
        challengeResponse.StatusCode = SearchResponseCodes.SearchFailed;
        mockChallengeService.Setup(s => s.GetChallengeQueryResultFromAccount(account, It.IsAny<IEnumerable<PayeScheme>>()))
            .Returns(challengeResponse);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.IsValid.Should().BeFalse();
        result.Characters.Should().BeEquivalentTo([command.FirstCharacterPosition, command.SecondCharacterPosition]);
        mockFinanceService.Verify(s => s.GetAccountBalances(It.IsAny<GetAccountBalancesRequest>()), Times.Never());
    }
}
