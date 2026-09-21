using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.ControllerTests.LearnersControllerTests;

[TestFixture]
internal class WhenPostingUpdateLearners
{
    [Test, MoqAutoData]
    public async Task UpdateLearners_ReturnsOk_WhenUpdateIsSuccessful(
        UpdateLearnerCostCommandResult expectedResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnerController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<UpdateLearnerCostCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await endpoint.UpdateLearner();

        // Assert
        result.Should().BeOfType<Ok<UpdateLearnerCostCommandResult>>();

        var okResult = result as Ok<UpdateLearnerCostCommandResult>;
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        mediator.Verify(
            m => m.Send(
                It.IsAny<UpdateLearnerCostCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Test, MoqAutoData]
    public async Task UpdateLearners_ReturnsInternalServerError_WhenUpdateThrowsException(
        Exception exception,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnerController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<UpdateLearnerCostCommand>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await endpoint.UpdateLearner();

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();

        mediator.Verify(
            m => m.Send(
                It.IsAny<UpdateLearnerCostCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}