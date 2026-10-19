using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Commands.ImportLearnersByAccountId;

[TestFixture]
internal class WhenHandlingImportLearnersByAccountIdCommand
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Success_When_There_Are_No_Apprenticeships(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, []);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.TotalRecordsProcessed.Should().Be(0);
        result.FailedRecordsCount.Should().Be(0);
        result.LastSuccessfulImportDate.Should().NotBeNull();
        result.FailureReason.Should().BeNull();
        financeClient.Verify(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()), Times.Never);
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(It.IsAny<PostApprenticeshipPaymentSummaryRequest>(), true), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Imports_Learners_And_Transfers(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        var transfer = CreateApprenticeship(2);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, [transfer]);
        SetupPayment(financeClient, learner.Id);
        SetupPayment(financeClient, transfer.Id);
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.TotalRecordsProcessed.Should().Be(2);
        result.FailedRecordsCount.Should().Be(0);
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PostApprenticeshipPaymentSummaryRequest>(r =>
                r.AccountId == command.AccountId &&
                r.ApprenticeshipId == learner.Id &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer == false), true), Times.Once);
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PostApprenticeshipPaymentSummaryRequest>(r =>
                r.AccountId == command.AccountId &&
                r.ApprenticeshipId == transfer.Id &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Payment_Details_When_A_Payment_Exists(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);
        SetupPayment(financeClient, learner.Id);
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PostApprenticeshipPaymentSummaryRequest>(r =>
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentAmount == 100 &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentDate == new DateOnly(2025, 1, 1) &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastUpdatedDate == new DateTime(2025, 1, 1)), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Posts_Summary_With_Null_Payment_Details_When_No_Payment_Exists(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);
        financeClient
            .Setup(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()))
            .ReturnsAsync((GetApprenticeshipPaymentByAccountIdResponse)null!);
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.TotalRecordsProcessed.Should().Be(1);
        result.FailedRecordsCount.Should().Be(0);
        VerifyPostedWithNullPaymentDetails(fundingProjectionClient, command.AccountId, learner);
    }

    [Test, MoqAutoData]
    public async Task Then_Posts_Summary_With_Null_Payment_Details_When_Payment_Has_No_ApprenticeshipId(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);
        financeClient
            .Setup(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()))
            .ReturnsAsync(new GetApprenticeshipPaymentByAccountIdResponse());
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.FailedRecordsCount.Should().Be(0);
        VerifyPostedWithNullPaymentDetails(fundingProjectionClient, command.AccountId, learner);
    }

    [Test]
    [MoqInlineAutoData(0, 2025)]
    [MoqInlineAutoData(13, 2025)]
    [MoqInlineAutoData(1, 0)]
    public async Task Then_Posts_Summary_With_Null_LastPaymentDate_When_Collection_Period_Is_Invalid(
        int collectionPeriodMonth,
        int collectionPeriodYear,
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);
        financeClient
            .Setup(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()))
            .ReturnsAsync(new GetApprenticeshipPaymentByAccountIdResponse
            {
                ApprenticeshipId = learner.Id,
                CollectionPeriodMonth = collectionPeriodMonth,
                CollectionPeriodYear = collectionPeriodYear,
                Amount = 100,
                LastUpdatedDate = new DateTime(2025, 1, 1)
            });
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.FailedRecordsCount.Should().Be(0);
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PostApprenticeshipPaymentSummaryRequest>(r =>
                r.ApprenticeshipId == learner.Id &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentDate == null &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentAmount == 100), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Reports_Failed_Records_And_Continues_When_One_Apprenticeship_Fails(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var successfulApprenticeship = CreateApprenticeship(1);
        var failedApprenticeship = CreateApprenticeship(2);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [successfulApprenticeship, failedApprenticeship]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);
        SetupPayment(financeClient, successfulApprenticeship.Id);
        financeClient
            .Setup(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(
                It.Is<GetApprenticeshipPaymentByAccountIdRequest>(r => r.ApprenticeshipId == failedApprenticeship.Id)))
            .ThrowsAsync(new InvalidOperationException("Payment lookup failed"));
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.TotalRecordsProcessed.Should().Be(2);
        result.FailedRecordsCount.Should().Be(1);
        result.LastSuccessfulImportDate.Should().BeNull();
        fundingProjectionClient.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(It.IsAny<PostApprenticeshipPaymentSummaryRequest>(), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Failure_When_Commitments_Cannot_Be_Read(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        commitmentsClient
            .Setup(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(It.IsAny<GetCommittedLearnersCostByAccountIdRequest>()))
            .ThrowsAsync(new InvalidOperationException("Commitments unavailable"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.TotalRecordsProcessed.Should().Be(0);
        result.FailedRecordsCount.Should().Be(0);
        result.FailureReason.Should().Be("Commitments unavailable");
        result.LastSuccessfulImportDate.Should().BeNull();
    }

    private static void SetupCommitmentsResponse(
        Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> client,
        long? accountId,
        long? transferSenderId,
        IEnumerable<GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse> apprenticeships)
    {
        var apprenticeshipList = apprenticeships.ToList();
        client
            .Setup(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r =>
                    r.AccountId == accountId &&
                    r.TransferSenderId == transferSenderId &&
                    r.PageNumber == 1 &&
                    r.PageItemCount == 100)))
            .ReturnsAsync(new GetCommittedLearnersCostByAccountIdResponse
            {
                Apprenticeships = apprenticeshipList,
                TotalApprenticeshipsFound = apprenticeshipList.Count
            });
    }

    private static void SetupPayment(
        Mock<IFinanceApiClient<FinanceApiConfiguration>> client,
        long apprenticeshipId)
    {
        client
            .Setup(c => c.Get<GetApprenticeshipPaymentByAccountIdResponse>(
                It.Is<GetApprenticeshipPaymentByAccountIdRequest>(r => r.ApprenticeshipId == apprenticeshipId)))
            .ReturnsAsync(new GetApprenticeshipPaymentByAccountIdResponse
            {
                ApprenticeshipId = apprenticeshipId,
                CollectionPeriodMonth = 1,
                CollectionPeriodYear = 2025,
                Amount = 100,
                LastUpdatedDate = new DateTime(2025, 1, 1)
            });
    }

    private static void VerifyPostedWithNullPaymentDetails(
        Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> client,
        long accountId,
        GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse apprenticeship)
    {
        client.Verify(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PostApprenticeshipPaymentSummaryRequest>(r =>
                r.AccountId == accountId &&
                r.ApprenticeshipId == apprenticeship.Id &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).TotalCost == apprenticeship.TotalAgreedPrice &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentAmount == null &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastPaymentDate == null &&
                ((PostApprenticeshipPaymentSummaryRequestData)r.Data).LastUpdatedDate == null), true), Times.Once);
    }

    private static void SetupSuccessfulPost(
        Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> client)
    {
        client
            .Setup(c => c.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
                It.IsAny<PostApprenticeshipPaymentSummaryRequest>(), true))
            .ReturnsAsync(new ApiResponse<ApprenticeshipPaymentSummaryResponse>(
                new ApprenticeshipPaymentSummaryResponse(), HttpStatusCode.OK, string.Empty));
    }

    private static GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse CreateApprenticeship(long id)
    {
        return new GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse
        {
            Id = id,
            Uln = "1234567890",
            DateOfBirth = new DateTime(2000, 1, 1),
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2026, 1, 1),
            ApprenticeshipStatus = ApprenticeshipStatus.Live,
            Cost = 1000,
            TotalAgreedPrice = 1200
        };
    }
}
