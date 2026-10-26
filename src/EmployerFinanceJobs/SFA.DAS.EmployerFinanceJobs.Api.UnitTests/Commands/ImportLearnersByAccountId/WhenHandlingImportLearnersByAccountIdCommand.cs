using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Apim.Shared.Interfaces;
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
        financeClient.Verify(c => c.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()), Times.Never);
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(It.IsAny<PutApprenticeshipPaymentSummaryRequest>()), Times.Never);
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
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PutApprenticeshipPaymentSummaryRequest>(r =>
                r.AccountId == command.AccountId &&
                r.ApprenticeshipId == learner.Id &&
                ((PutApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer == false)), Times.Once);
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PutApprenticeshipPaymentSummaryRequest>(r =>
                r.AccountId == command.AccountId &&
                r.ApprenticeshipId == transfer.Id &&
                ((PutApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Excludes_Learners_Funded_By_A_Transfer_From_Another_Account(
        ImportLearnersByAccountIdCommand command,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var learner = CreateApprenticeship(1);
        var transferFundedLearner = CreateApprenticeship(2);
        transferFundedLearner.TransferSenderId = command.AccountId + 1;
        // Transfers out always have this account as the sender, so they must not be filtered
        var transferOut = CreateApprenticeship(3);
        transferOut.TransferSenderId = command.AccountId;
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, [learner, transferFundedLearner]);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, [transferOut]);
        SetupPayment(financeClient, learner.Id);
        SetupPayment(financeClient, transferOut.Id);
        SetupSuccessfulPost(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.TotalRecordsProcessed.Should().Be(2);
        result.FailedRecordsCount.Should().Be(0);
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PutApprenticeshipPaymentSummaryRequest>(r =>
                r.ApprenticeshipId == learner.Id &&
                ((PutApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer == false)), Times.Once);
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PutApprenticeshipPaymentSummaryRequest>(r =>
                r.ApprenticeshipId == transferOut.Id &&
                ((PutApprenticeshipPaymentSummaryRequestData)r.Data).IsTransfer)), Times.Once);
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
            It.Is<PutApprenticeshipPaymentSummaryRequest>(r => r.ApprenticeshipId == transferFundedLearner.Id)), Times.Never);
        financeClient.Verify(c => c.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(
            It.Is<GetApprenticeshipPaymentByAccountIdRequest>(r => r.ApprenticeshipId == transferFundedLearner.Id)), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Total_Paid_And_Last_Payment_From_All_Payments(
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
        SetupPayments(financeClient, learner.Id,
            CreatePayment(learner.Id, 2025, 1, 100, new DateTime(2025, 2, 1)),
            CreatePayment(learner.Id, 2025, 3, 150, new DateTime(2025, 4, 1)),
            // Late correction for an earlier period should not be treated as the last payment
            CreatePayment(learner.Id, 2024, 12, 50, new DateTime(2025, 6, 1)));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        var data = posted.Should().ContainSingle().Subject;
        data.TotalCost.Should().Be(learner.TotalAgreedPrice);
        data.TotalPaid.Should().Be(300);
        data.LastPaymentAmount.Should().Be(150);
        data.LastPaymentDate.Should().Be(new DateOnly(2025, 3, 1));
        data.LastUpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Test, MoqAutoData]
    public async Task Then_Uses_The_Most_Recently_Updated_Payment_When_Collection_Periods_Match(
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
        SetupPayments(financeClient, learner.Id,
            CreatePayment(learner.Id, 2025, 3, 150, new DateTime(2025, 4, 1)),
            CreatePayment(learner.Id, 2025, 3, 175, new DateTime(2025, 4, 2)));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var data = posted.Should().ContainSingle().Subject;
        data.TotalPaid.Should().Be(325);
        data.LastPaymentAmount.Should().Be(175);
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task Then_Posts_Summary_With_No_Payment_Details_When_No_Payments_Exist(
        bool financeReturnsNull,
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
            .Setup(c => c.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(It.IsAny<GetApprenticeshipPaymentByAccountIdRequest>()))
            .ReturnsAsync(financeReturnsNull ? null! : []);
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.TotalRecordsProcessed.Should().Be(1);
        result.FailedRecordsCount.Should().Be(0);
        var data = posted.Should().ContainSingle().Subject;
        data.TotalCost.Should().Be(learner.TotalAgreedPrice);
        data.TotalPaid.Should().Be(0);
        data.LastPaymentAmount.Should().BeNull();
        data.LastPaymentDate.Should().BeNull();
        data.LastUpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
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
        SetupPayments(financeClient, learner.Id,
            CreatePayment(learner.Id, collectionPeriodYear, collectionPeriodMonth, 100, new DateTime(2025, 1, 1)));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.FailedRecordsCount.Should().Be(0);
        var data = posted.Should().ContainSingle().Subject;
        data.LastPaymentDate.Should().BeNull();
        data.LastPaymentAmount.Should().Be(100);
        data.TotalPaid.Should().Be(100);
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
            .Setup(c => c.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(
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
        fundingProjectionClient.Verify(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(It.IsAny<PutApprenticeshipPaymentSummaryRequest>()), Times.Once);
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
        SetupPayments(client, apprenticeshipId, CreatePayment(apprenticeshipId, 2025, 1, 100, new DateTime(2025, 1, 1)));
    }

    private static void SetupPayments(
        Mock<IFinanceApiClient<FinanceApiConfiguration>> client,
        long apprenticeshipId,
        params GetApprenticeshipPaymentByAccountIdResponse[] payments)
    {
        client
            .Setup(c => c.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(
                It.Is<GetApprenticeshipPaymentByAccountIdRequest>(r => r.ApprenticeshipId == apprenticeshipId)))
            .ReturnsAsync(payments.ToList());
    }

    private static GetApprenticeshipPaymentByAccountIdResponse CreatePayment(
        long apprenticeshipId,
        int collectionPeriodYear,
        int collectionPeriodMonth,
        decimal amount,
        DateTime lastUpdatedDate)
    {
        return new GetApprenticeshipPaymentByAccountIdResponse
        {
            ApprenticeshipId = apprenticeshipId,
            CollectionPeriodYear = collectionPeriodYear,
            CollectionPeriodMonth = collectionPeriodMonth,
            Amount = amount,
            LastUpdatedDate = lastUpdatedDate,
        };
    }

    private static List<PutApprenticeshipPaymentSummaryRequestData> CapturePostedSummaries(
        Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> client)
    {
        var posted = new List<PutApprenticeshipPaymentSummaryRequestData>();
        client
            .Setup(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
                It.IsAny<PutApprenticeshipPaymentSummaryRequest>()))
            .Callback<IPutApiRequest>((r) => posted.Add((PutApprenticeshipPaymentSummaryRequestData)r.Data))
            .ReturnsAsync(new ApiResponse<ApprenticeshipPaymentSummaryResponse>(
                new ApprenticeshipPaymentSummaryResponse(), HttpStatusCode.OK, string.Empty));
        return posted;
    }

    private static void SetupSuccessfulPost(
        Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> client)
    {
        client
            .Setup(c => c.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(
                It.IsAny<PutApprenticeshipPaymentSummaryRequest>()))
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
            TotalAgreedPrice = 1200,
            FreezedDate = null
        };
    }
}