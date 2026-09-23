using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Application.Command;

[TestFixture]
internal class WhenHandlingRecalculateFundingProjectionCommand
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsExpectedResult_WhenJobStateExistsAndRecalculationSucceeds(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        RecalculateFundingProjectionApiResponse recalculationResult,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.OK, null));

        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()))
            .ReturnsAsync(new ApiResponse<RecalculateFundingProjectionApiResponse>(recalculationResult, HttpStatusCode.OK, null));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalRecordsProcessed.Should().Be(recalculationResult.TotalRecordsProcessed);
        result.TotalRecordsUpdated.Should().Be(recalculationResult.TotalRecordsUpdated);
        result.TotalRecordsInserted.Should().Be(recalculationResult.TotalRecordsInserted);
    }

    [Test, MoqAutoData]
    public async Task Handle_ReturnsExpectedResult_WhenJobStateIsCreated(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        RecalculateFundingProjectionApiResponse recalculationResult,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.Created, null));

        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()))
            .ReturnsAsync(new ApiResponse<RecalculateFundingProjectionApiResponse>(recalculationResult, HttpStatusCode.OK, null));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalRecordsProcessed.Should().Be(recalculationResult.TotalRecordsProcessed);
        result.TotalRecordsUpdated.Should().Be(recalculationResult.TotalRecordsUpdated);
        result.TotalRecordsInserted.Should().Be(recalculationResult.TotalRecordsInserted);
    }

    [Test, MoqAutoData]
    public async Task Handle_PassesLastSuccessfulImportDateAsCutOff(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        RecalculateFundingProjectionApiResponse recalculationResult,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.OK, null));

        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()))
            .ReturnsAsync(new ApiResponse<RecalculateFundingProjectionApiResponse>(recalculationResult, HttpStatusCode.OK, null));

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        fundingProjectionApiClient.Verify(
            x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.Is<RecalculateFundingProjectionApiRequest>(
                    r => r.CutOffDateTime == jobState.LastSuccessfulImportDate)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_ThrowsInvalidOperationException_WhenGetOrCreateJobStateFails(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.InternalServerError, null));

        // Act
        Func<Task> act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{HttpStatusCode.InternalServerError}*");

        fundingProjectionApiClient.Verify(
            x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_ThrowsInvalidOperationException_WhenRecalculationFails(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        RecalculateFundingProjectionApiResponse recalculationResult,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.OK, null));

        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()))
            .ReturnsAsync(new ApiResponse<RecalculateFundingProjectionApiResponse>(recalculationResult, HttpStatusCode.InternalServerError, null));

        // Act
        Func<Task> act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{HttpStatusCode.InternalServerError}*");
    }

    [Test, MoqAutoData]
    public async Task Handle_ThrowsException_WhenJobStateApiThrows(
        RecalculateFundingProjectionCommand request,
        Exception exception,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ThrowsAsync(exception);

        // Act
        Func<Task> act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .Where(x => x == exception);

        fundingProjectionApiClient.Verify(
            x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_ThrowsException_WhenRecalculationApiThrows(
        RecalculateFundingProjectionCommand request,
        ImportJobStateApiResponse jobState,
        Exception exception,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] RecalculateFundingProjectionCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<ImportJobStateApiResponse>(
                It.IsAny<GetOrCreateJobApiRequest>()))
            .ReturnsAsync(new ApiResponse<ImportJobStateApiResponse>(jobState, HttpStatusCode.OK, null));

        fundingProjectionApiClient
            .Setup(x => x.PostWithResponseCode<RecalculateFundingProjectionApiResponse>(
                It.IsAny<RecalculateFundingProjectionApiRequest>()))
            .ThrowsAsync(exception);

        // Act
        Func<Task> act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .Where(x => x == exception);
    }
}
