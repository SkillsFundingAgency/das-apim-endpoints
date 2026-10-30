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
    public async Task Then_The_Job_Is_Put_To_Funding_Projection(
        PostJobCommand command,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] PostJobCommandHandler handler)
    {
        // Arrange
        fundingProjectionClient
            .Setup(client => client.PutWithResponseCode<NullResponse>(
                It.IsAny<PutJobRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        fundingProjectionClient.Verify(client => client.PutWithResponseCode<NullResponse>(
            It.Is<PutJobRequest>(request =>
                request.Id == command.Id &&
                request.PutUrl == $"api/jobs/{command.Id}" &&
                ((PutJobRequestData)request.Data).AccountId == command.Data.AccountId &&
                ((PutJobRequestData)request.Data).JobName == command.Data.JobName &&
                ((PutJobRequestData)request.Data).JobState == command.Data.JobState &&
                ((PutJobRequestData)request.Data).LastSuccessfulImportDate == command.Data.LastSuccessfulImportDate &&
                ((PutJobRequestData)request.Data).LastAttemptedDate == command.Data.LastAttemptedDate &&
                ((PutJobRequestData)request.Data).LastAttemptSuccessful == command.Data.LastAttemptSuccessful &&
                ((PutJobRequestData)request.Data).TotalRecordsLastRun == command.Data.TotalRecordsLastRun &&
                ((PutJobRequestData)request.Data).FailedRecordsLastRun == command.Data.FailedRecordsLastRun)),
            Times.Once);
    }
}
