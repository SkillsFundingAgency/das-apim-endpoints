using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Controllers;

[TestFixture]
internal class ApprenticeshipControllerTests
{
    [Test, MoqAutoData]
    public async Task ImportLearnersByAccountId_Sends_Command_And_Returns_Result(
        PostImportLearnersByAccountIdRequest request,
        ImportLearnersByAccountIdResult expectedResult,
        Mock<IMediator> mediator)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(
                It.Is<ImportLearnersByAccountIdCommand>(c => c.AccountId == request.AccountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var controller = new ApprenticeshipController(
            NullLogger<ApprenticeshipController>.Instance,
            mediator.Object);

        // Act
        var result = await controller.ImportLearnersByAccountId(request);

        // Assert
        var okResult = result.Should().BeOfType<Ok<ImportLearnersByAccountIdResult>>().Subject;
        okResult.Value.Should().BeSameAs(expectedResult);
        mediator.Verify(x => x.Send(
            It.Is<ImportLearnersByAccountIdCommand>(c => c.AccountId == request.AccountId),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
