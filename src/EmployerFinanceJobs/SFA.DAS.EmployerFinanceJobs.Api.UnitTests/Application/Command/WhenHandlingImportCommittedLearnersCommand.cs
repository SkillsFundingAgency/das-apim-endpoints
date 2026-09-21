using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Application.Command;

[TestFixture]
internal class WhenHandlingImportCommittedLearnersCommand
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsZeroResults_WhenNoLearnersAreFound(
        ImportCommittedLearnersCommand request,
        ImportJobStateApiResponse importJobStateApiResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] ImportCommittedLearnersCommandHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        commitmentsApiClient
            .Setup(x => x.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(
                It.IsAny<DateTime>(),
                1,
                It.IsAny<int>())))
            .ReturnsAsync(new GetAllLearnersApiResponse
            {
                Learners = []
            });

        fundingProjectionApiClient.Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(importJobStateApiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.BatchesProcessed.Should().Be(0);
        result.TotalRecords.Should().Be(0);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_ReturnsZeroResults_WhenFirstBatchIsNull(
        ImportCommittedLearnersCommand request,
        ImportJobStateApiResponse importJobStateApiResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] ImportCommittedLearnersCommandHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        commitmentsApiClient
            .Setup(x => x.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(
                It.IsAny<DateTime>(),
                1,
                It.IsAny<int>())))
            .ReturnsAsync((GetAllLearnersApiResponse)null!);

        fundingProjectionApiClient.Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(importJobStateApiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.BatchesProcessed.Should().Be(0);
        result.TotalRecords.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Handle_UsesLastSuccessfulImportDate_WhenFetchingFirstBatch(
        ImportCommittedLearnersCommand request,
        DateTime lastSuccessfulImportDate,
        GetAllLearnersApiResponse firstBatch,
        ImportJobStateApiResponse importJobStateApiResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] ImportCommittedLearnersCommandHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        lastSuccessfulImportDate = importJobStateApiResponse.LastSuccessfulImportDate;
        commitmentsApiClient
            .Setup(x => x.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(
                lastSuccessfulImportDate,
                1000,
                1)))
            .ReturnsAsync(firstBatch);

        fundingProjectionApiClient.Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(importJobStateApiResponse, HttpStatusCode.OK, ""));

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        commitmentsApiClient.Verify(
            x => x.Get<GetAllLearnersApiResponse>(new GetAllLearnersApiRequest(
                lastSuccessfulImportDate,
                1000,
                1)),
            Times.Once);
    }
}