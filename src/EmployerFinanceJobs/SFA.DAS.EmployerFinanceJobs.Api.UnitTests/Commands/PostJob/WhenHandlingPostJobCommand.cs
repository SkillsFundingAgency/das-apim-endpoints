using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinanceJobs.Commands.PostJob;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Commands.PostJob;

[TestFixture]
internal class WhenHandlingPostJobCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Job_Is_Posted_To_Funding_Projection(
        PostJobCommand command,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] PostJobCommandHandler handler)
    {
        // Arrange
        fundingProjectionClient
            .Setup(client => client.PostWithResponseCode<NullResponse>(
                It.IsAny<PostJobRequest>(), true))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        fundingProjectionClient.Verify(client => client.PostWithResponseCode<NullResponse>(
            It.Is<PostJobRequest>(request =>
                request.Id == command.Id &&
                request.PostUrl == $"api/jobs/{command.Id}" &&
                ((PostJobRequestData)request.Data).AccountId == command.Data.AccountId &&
                ((PostJobRequestData)request.Data).JobName == command.Data.JobName &&
                ((PostJobRequestData)request.Data).JobState == command.Data.JobState &&
                ((PostJobRequestData)request.Data).LastSuccessfulImportDate == command.Data.LastSuccessfulImportDate &&
                ((PostJobRequestData)request.Data).LastAttemptedDate == command.Data.LastAttemptedDate &&
                ((PostJobRequestData)request.Data).LastAttemptSuccessful == command.Data.LastAttemptSuccessful &&
                ((PostJobRequestData)request.Data).TotalRecordsLastRun == command.Data.TotalRecordsLastRun &&
                ((PostJobRequestData)request.Data).FailedRecordsLastRun == command.Data.FailedRecordsLastRun), true),
            Times.Once);
    }
}
