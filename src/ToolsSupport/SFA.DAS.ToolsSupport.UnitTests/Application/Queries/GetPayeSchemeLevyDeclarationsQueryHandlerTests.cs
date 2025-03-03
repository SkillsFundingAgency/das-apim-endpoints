using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;
using SFA.DAS.ToolsSupport.Configuration;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;
public class GetPayeSchemeLevyDeclarationsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnSuccessResult_WhenAllDataIsValid(
        GetPayeSchemeLevyDeclarationsQuery query,
        Account account,
        string actualPayeId,
        PayeScheme payeScheme,
        PayeSchemeLevyDeclarations levyDeclarations,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IHmrcApiClient<HmrcApiConfiguration>> mockHmrcApiClient,
        GetPayeSchemeLevyDeclarationsQueryHandler handler)
    {
        // Arrange
        account.PayeSchemes = [new() { Id = actualPayeId, Href = "/paye/123" }];

        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync(account);
        mockAccountsService.Setup(s => s.GetEmployerAccountPayeScheme("/paye/123")).ReturnsAsync(payeScheme);

        levyDeclarations.Declarations =
            [
                new Declaration { SubmissionTime = new DateTime(2018, 1, 1), Id = 1 },
                new Declaration { SubmissionTime = new DateTime(2017, 5, 1), Id = 2 }
            ];

        mockHmrcApiClient.Setup(s => s.Get<PayeSchemeLevyDeclarations>(It.IsAny<GetPayeSchemeLevyDeclarationsRequest>()))
            .ReturnsAsync(levyDeclarations);
      
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(PayeLevySubmissionsResponseCodes.Success);
        result.PayeScheme.Should().Be(payeScheme);
        result.LevySubmissions.Should().NotBeNull();
        result.LevySubmissions.Declarations.Should().HaveCount(2);
        result.LevySubmissions.Declarations[0].SubmissionTime.Should().Be(new DateTime(2018, 1, 1));
        result.LevySubmissions.Declarations[1].SubmissionTime.Should().Be(new DateTime(2017, 5, 1));

        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockAccountsService.Verify(s => s.GetEmployerAccountPayeScheme("/paye/123"), Times.Once());
        mockHmrcApiClient.Verify(s => s.Get<PayeSchemeLevyDeclarations>(It.IsAny<GetPayeSchemeLevyDeclarationsRequest>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnAccountNotFound_WhenAccountIsNull(
            GetPayeSchemeLevyDeclarationsQuery query,
            [Frozen] Mock<IAccountsService> mockAccountsService,
            [Frozen] Mock<IPayRefHashingService> mockHashingService,
            [Frozen] Mock<IHmrcApiClient<HmrcApiConfiguration>> mockHmrcApiClient,
            GetPayeSchemeLevyDeclarationsQueryHandler handler)
    {
        // Arrange
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync((Account)null);
    
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(PayeLevySubmissionsResponseCodes.AccountNotFound);
        result.PayeScheme.Should().BeEquivalentTo(new PayeScheme());
        result.LevySubmissions.Should().BeEquivalentTo(new PayeSchemeLevyDeclarations());
        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockHashingService.Verify(s => s.DecodeValueToString(It.IsAny<string>()), Times.Never());
        mockHmrcApiClient.Verify(s => s.Get<PayeSchemeLevyDeclarations>(It.IsAny<GetPayeSchemeLevyDeclarationsRequest>()), Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Handle_ShouldReturnDefaultResult_WhenHmrcApiReturnsNull(
        GetPayeSchemeLevyDeclarationsQuery query,
        Account account,
        string actualPayeId,
        PayeScheme payeScheme,
        [Frozen] Mock<IAccountsService> mockAccountsService,
        [Frozen] Mock<IHmrcApiClient<HmrcApiConfiguration>> mockHmrcApiClient,
        GetPayeSchemeLevyDeclarationsQueryHandler handler)
    {
        // Arrange
        account.PayeSchemes = [new () { Id = actualPayeId, Href = "/paye/123" }];
        mockAccountsService.Setup(s => s.GetAccount(query.AccountId)).ReturnsAsync(account);
        mockAccountsService.Setup(s => s.GetEmployerAccountPayeScheme("/paye/123")).ReturnsAsync(payeScheme);

        mockHmrcApiClient.Setup(s => s.Get<PayeSchemeLevyDeclarations>(It.IsAny<GetPayeSchemeLevyDeclarationsRequest>()))
            .ReturnsAsync((PayeSchemeLevyDeclarations)null);
      
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(PayeLevySubmissionsResponseCodes.Success);
        result.PayeScheme.Should().Be(payeScheme);
        result.LevySubmissions.Should().NotBeNull();
        result.LevySubmissions.Declarations.Should().BeEmpty();
        mockAccountsService.Verify(s => s.GetAccount(query.AccountId), Times.Once());
        mockHmrcApiClient.Verify(s => s.Get<PayeSchemeLevyDeclarations>(It.IsAny<GetPayeSchemeLevyDeclarationsRequest>()), Times.Once());
    }
}
