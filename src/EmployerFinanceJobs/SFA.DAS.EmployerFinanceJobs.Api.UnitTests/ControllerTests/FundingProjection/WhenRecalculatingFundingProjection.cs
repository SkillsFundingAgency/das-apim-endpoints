using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.ControllerTests.FundingProjection;

[TestFixture]
internal class WhenRecalculatingFundingProjection
{
    [Test, MoqAutoData]
    public async Task Recalculate_ReturnsOk_WhenRecalculateIsSuccessful(
        RecalculateFundingProjectionCommandResult expectedResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<RecalculateFundingProjectionCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await endpoint.RecalculateFundingProjection();

        // Assert
        result.Should().BeOfType<Ok<RecalculateFundingProjectionCommandResult>>();

        var okResult = result as Ok<RecalculateFundingProjectionCommandResult>;
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        mediator.Verify(
            m => m.Send(
                It.IsAny<RecalculateFundingProjectionCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Test, MoqAutoData]
    public async Task Recalculate_ReturnsInternalServerError_WhenRecalculateThrowsException(
        Exception exception,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FundingProjectionController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<RecalculateFundingProjectionCommand>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await endpoint.RecalculateFundingProjection();

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();

        mediator.Verify(
            m => m.Send(
                It.IsAny<RecalculateFundingProjectionCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}