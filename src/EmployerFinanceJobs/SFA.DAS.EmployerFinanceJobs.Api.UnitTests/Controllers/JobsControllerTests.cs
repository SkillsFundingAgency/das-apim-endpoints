using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;
using SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;
using SFA.DAS.EmployerFinanceJobs.Commands.PostJob;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Controllers;

[TestFixture]
internal class JobsControllerTests
{
    [Test, MoqAutoData]
    public async Task PostJob_Sends_The_Route_Id_And_Body_To_The_Handler(
        long id,
        PostJobRequest request,
        Mock<IMediator> mediator)
    {
        // Arrange
        mediator
            .Setup(client => client.Send(
                It.Is<PostJobCommand>(command =>
                    command.Id == id &&
                    command.Data.AccountId == request.AccountId &&
                    command.Data.JobName == request.JobName &&
                    command.Data.LastSuccessfulImportDate == request.LastSuccessfulImportDate &&
                    command.Data.LastAttemptedDate == request.LastAttemptedDate &&
                    command.Data.LastAttemptSuccessful == request.LastAttemptSuccessful &&
                    command.Data.TotalRecordsLastRun == request.TotalRecordsLastRun &&
                    command.Data.FailedRecordsLastRun == request.FailedRecordsLastRun),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var controller = new JobsController(mediator.Object);

        // Act
        var result = await controller.PostJob(id, request);

        // Assert
        result.Should().BeOfType<Ok>();
        mediator.Verify(client => client.Send(
            It.Is<PostJobCommand>(command => command.Id == id),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
