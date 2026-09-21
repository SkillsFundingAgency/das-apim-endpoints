using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Application.Command;

[TestFixture]
internal class WhenHandlingUpdateLearnersCommand
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsZeroRecords_WhenNoPendingLearnersFound(
        UpdateLearnerCostCommand request,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] UpdateLearnerCostCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.Get<List<GetCommittedLearnerApiResponse>>(
                It.Is<GetPendingImportLearnersByStatusApiRequest>(
                    r => r.Status == "Pending")))
            .ReturnsAsync([]);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalRecords.Should().Be(0);
        result.SuccessfulRecords.Should().Be(0);
        result.FailedRecords.Should().Be(0);

        fundingProjectionApiClient.Verify(
            x => x.Get<List<GetCommittedLearnerApiResponse>>(
                It.Is<GetPendingImportLearnersByStatusApiRequest>(
                    r => r.Status == "Pending")),
            Times.Once);
    }


    [Test, MoqAutoData]
    public async Task Handle_ReturnsZeroRecords_WhenPendingLearnersIsNull(
        UpdateLearnerCostCommand request,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] UpdateLearnerCostCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.Get<List<GetCommittedLearnerApiResponse>>(
                It.IsAny<GetPendingImportLearnersByStatusApiRequest>()))
            .ReturnsAsync((List<GetCommittedLearnerApiResponse>?)null!);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.TotalRecords.Should().Be(0);
        result.SuccessfulRecords.Should().Be(0);
        result.FailedRecords.Should().Be(0);
    }


    [Test, MoqAutoData]
    public async Task Handle_ThrowsException_WhenGettingPendingLearnersFails(
        UpdateLearnerCostCommand request,
        Exception exception,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        [Greedy] UpdateLearnerCostCommandHandler handler)
    {
        // Arrange
        fundingProjectionApiClient
            .Setup(x => x.Get<List<GetCommittedLearnerApiResponse>>(
                It.IsAny<GetPendingImportLearnersByStatusApiRequest>()))
            .ThrowsAsync(exception);

        // Act
        Func<Task> act = () =>
            handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .Where(x => x == exception);

        fundingProjectionApiClient.Verify(
            x => x.Get<List<GetCommittedLearnerApiResponse>>(
                It.IsAny<GetPendingImportLearnersByStatusApiRequest>()),
            Times.Once);
    }
}