using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.ControllerTests;

[TestFixture]
public class FundingProjectionControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Ok_Result_Is_Returned_With_Projection_Data(
        long accountId,
        GetEmployerFundingProjectionByAccountIdQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(
                It.Is<GetEmployerFundingProjectionByAccountIdQuery>(q => q.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // Act
        var actual = await controller.GetFundingProjection(accountId, CancellationToken.None);

        // Assert
        actual.Should().BeOfType<Ok<GetEmployerFundingProjectionByAccountIdQueryResult>>();
        var okResult = actual as Ok<GetEmployerFundingProjectionByAccountIdQueryResult>;
        okResult!.Value.Should().BeEquivalentTo(queryResult);
    }

    [Test, MoqAutoData]
    public async Task Then_Mediator_Is_Called_With_Correct_AccountId(
        long accountId,
        GetEmployerFundingProjectionByAccountIdQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmployerFundingProjectionByAccountIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        // Act
        await controller.GetFundingProjection(accountId, CancellationToken.None);

        // Assert
        mediator.Verify(x => x.Send(
            It.Is<GetEmployerFundingProjectionByAccountIdQuery>(q => q.AccountId == accountId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_InternalServerError_Is_Returned_When_Mediator_Throws(
        long accountId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmployerFundingProjectionByAccountIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var actual = await controller.GetFundingProjection(accountId, CancellationToken.None);

        // Assert
        actual.Should().BeOfType<StatusCodeHttpResult>();
        var statusCodeResult = actual as StatusCodeHttpResult;
        statusCodeResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Error_Is_Logged_When_Exception_Is_Thrown(
        long accountId,
        Exception exception,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ILogger<FundingProjectionController>> logger,
        [Greedy] FundingProjectionController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmployerFundingProjectionByAccountIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        await controller.GetFundingProjection(accountId, CancellationToken.None);

        // Assert
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error getting employer funding projection")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Ok_Result_Is_Returned_When_Mediator_Returns_Null(
        long accountId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmployerFundingProjectionByAccountIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetEmployerFundingProjectionByAccountIdQueryResult)null!);

        // Act
        var actual = await controller.GetFundingProjection(accountId, CancellationToken.None);

        // Assert
        actual.Should().BeOfType<Ok<GetEmployerFundingProjectionByAccountIdQueryResult>>();
        var okResult = actual as Ok<GetEmployerFundingProjectionByAccountIdQueryResult>;
        okResult!.Value.Should().BeNull();
    }
}