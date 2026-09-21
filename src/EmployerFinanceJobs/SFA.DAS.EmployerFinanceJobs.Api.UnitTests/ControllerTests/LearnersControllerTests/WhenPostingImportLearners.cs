using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.ControllerTests.LearnersControllerTests;

[TestFixture]
internal class WhenPostingImportLearners
{
    [Test, MoqAutoData]
    public async Task ImportLearners_ReturnsOk_WhenImportIsSuccessful(
        ImportCommittedLearnersCommandResult expectedResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnerController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<ImportCommittedLearnersCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await endpoint.ImportLearners();

        // Assert
        result.Should().BeOfType<Ok<ImportCommittedLearnersCommandResult>>();

        var okResult = result as Ok<ImportCommittedLearnersCommandResult>;
        okResult!.Value.Should().BeEquivalentTo(expectedResult);

        mediator.Verify(
            m => m.Send(
                It.IsAny<ImportCommittedLearnersCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Test, MoqAutoData]
    public async Task ImportLearners_ReturnsInternalServerError_WhenImportThrowsException(
        Exception exception,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnerController endpoint)
    {
        // Arrange
        mediator
            .Setup(m => m.Send(
                It.IsAny<ImportCommittedLearnersCommand>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await endpoint.ImportLearners();

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();

        mediator.Verify(
            m => m.Send(
                It.IsAny<ImportCommittedLearnersCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}