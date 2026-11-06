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
using System.Net;

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

    [Test]
    [MoqInlineAutoData(null)]
    [MoqInlineAutoData("0001-01-01T00:00:00")]
    public async Task Then_All_Learners_Are_Requested_When_The_Account_Has_Never_Been_Imported(
        string? lastSuccessfulImportDate,
        long accountId,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var command = new ImportLearnersByAccountIdCommand(accountId,
            lastSuccessfulImportDate is null ? null : DateTime.Parse(lastSuccessfulImportDate));
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, []);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.FailureReason.Should().BeNull();
        commitmentsClient.Verify(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
            It.Is<GetCommittedLearnersCostByAccountIdRequest>(r => r.Statuses != null && r.LastUpdated == null)), Times.Exactly(2));
    }

    [Test, MoqAutoData]
    public async Task Then_Learners_Updated_Since_The_Day_Before_The_Last_Successful_Import_Are_Requested(
        long accountId,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var lastSuccessfulImportDate = new DateTime(2026, 3, 15, 10, 30, 0, DateTimeKind.Utc);
        var command = new ImportLearnersByAccountIdCommand(accountId, lastSuccessfulImportDate);
        SetupCommitmentsResponse(commitmentsClient, command.AccountId, null, []);
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        commitmentsClient.Verify(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
            It.Is<GetCommittedLearnersCostByAccountIdRequest>(r => r.LastUpdated == lastSuccessfulImportDate.AddDays(-1))), Times.Exactly(2));
    }

    [Test, MoqAutoData]
    public async Task Then_Every_Page_Is_Requested_With_The_Same_Last_Updated_Filter(
        long accountId,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsClient,
        [Greedy] ImportLearnersByAccountIdCommandHandler handler)
    {
        // Arrange
        var lastSuccessfulImportDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc);
        var command = new ImportLearnersByAccountIdCommand(accountId, lastSuccessfulImportDate);
        commitmentsClient
            .Setup(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r => r.AccountId == accountId && r.Statuses != null)))
            .ReturnsAsync(new GetCommittedLearnersCostByAccountIdResponse
            {
                Apprenticeships = [],
                TotalApprenticeshipsFound = 150
            });
        commitmentsClient
            .Setup(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r => r.AccountId == accountId && r.PageNumber == 1)))
            .ReturnsAsync(new GetCommittedLearnersCostByAccountIdResponse
            {
                Apprenticeships = [CreateApprenticeship(1)],
                TotalApprenticeshipsFound = 150
            });
        SetupCommitmentsResponse(commitmentsClient, null, command.AccountId, []);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        commitmentsClient.Verify(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
            It.Is<GetCommittedLearnersCostByAccountIdRequest>(r =>
                r.AccountId == accountId &&
                r.PageNumber == 2 &&
                r.LastUpdated == lastSuccessfulImportDate.AddDays(-1))), Times.Once);
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId);
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
    public async Task Then_Maps_Provider_Order_From_Provider_Payments_Priority(
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId,
            new GetProviderPaymentsPriorityResponse.ProviderPaymentPriorityItem
            {
                ProviderId = learner.ProviderId,
                PriorityOrder = 3
            });
        SetupPayment(financeClient, learner.Id);
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        posted.Should().ContainSingle().Which.ProviderOrder.Should().Be(3);
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId);
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
        // Collection period March lands in the account in April
        data.LastPaymentDate.Should().Be(new DateOnly(2025, 4, 1));
        data.LastUpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Test, MoqAutoData]
    public async Task Then_Sums_All_Payments_In_The_Last_Collection_Period(
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId);
        SetupPayments(financeClient, learner.Id,
            CreatePayment(learner.Id, 2025, 2, 100, new DateTime(2025, 3, 1)),
            CreatePayment(learner.Id, 2025, 3, 150, new DateTime(2025, 4, 1)),
            CreatePayment(learner.Id, 2025, 3, 175, new DateTime(2025, 4, 2)));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var data = posted.Should().ContainSingle().Subject;
        data.TotalPaid.Should().Be(425);
        data.LastPaymentAmount.Should().Be(325);
        // Collection period March lands in the account in April
        data.LastPaymentDate.Should().Be(new DateOnly(2025, 4, 1));
    }

    [Test, MoqAutoData]
    public async Task Then_Every_Funding_Source_Except_Fully_Funded_Sfa_Counts_Towards_The_Payments(
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
            CreatePayment(learner.Id, 2025, 2, 100, new DateTime(2025, 3, 1), FundingSource.Levy),
            CreatePayment(learner.Id, 2025, 3, 100, new DateTime(2025, 4, 1), FundingSource.Levy),
            CreatePayment(learner.Id, 2025, 3, 95, new DateTime(2025, 4, 1), FundingSource.CoInvestedSfa),
            CreatePayment(learner.Id, 2025, 3, 5, new DateTime(2025, 4, 1), FundingSource.CoInvestedEmployer),
            CreatePayment(learner.Id, 2025, 3, 50, new DateTime(2025, 4, 1), FundingSource.LevyTransfer),
            CreatePayment(learner.Id, 2025, 3, 1000, new DateTime(2025, 4, 1), FundingSource.FullyFundedSfa));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var data = posted.Should().ContainSingle().Subject;
        data.TotalPaid.Should().Be(350);
        data.LastPaymentAmount.Should().Be(250);
    }

    [Test, MoqAutoData]
    public async Task Then_Only_Fully_Funded_Sfa_Payments_Are_Treated_As_No_Payments(
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
            CreatePayment(learner.Id, 2025, 3, 1000, new DateTime(2025, 4, 1), FundingSource.FullyFundedSfa));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var data = posted.Should().ContainSingle().Subject;
        data.TotalPaid.Should().Be(0);
        data.LastPaymentAmount.Should().BeNull();
        data.LastPaymentDate.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_A_December_Collection_Period_Is_Paid_In_January_Of_The_Next_Year(
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
            CreatePayment(learner.Id, 2025, 12, 100, new DateTime(2026, 1, 6)));
        var posted = CapturePostedSummaries(fundingProjectionClient);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var data = posted.Should().ContainSingle().Subject;
        data.LastPaymentDate.Should().Be(new DateOnly(2026, 1, 1));
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId);
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
        SetupProviderPaymentsPriority(commitmentsClient, command.AccountId);
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

    private static void SetupProviderPaymentsPriority(
        Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> client,
        long accountId,
        params List<GetProviderPaymentsPriorityResponse.ProviderPaymentPriorityItem> priorities)
    {
        client
            .Setup(c => c.Get<GetProviderPaymentsPriorityResponse>(
                It.IsAny<GetProviderPaymentsPriorityRequest>()))
            .ReturnsAsync(new GetProviderPaymentsPriorityResponse
            {
                ProviderPaymentPriorities = [.. priorities.OrderBy(p => p.PriorityOrder)]
            });
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
        DateTime lastUpdatedDate,
        FundingSource fundingSource = FundingSource.Levy)
    {
        return new GetApprenticeshipPaymentByAccountIdResponse
        {
            ApprenticeshipId = apprenticeshipId,
            CollectionPeriodYear = collectionPeriodYear,
            CollectionPeriodMonth = collectionPeriodMonth,
            FundingSource = fundingSource,
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
            .Callback<IPutApiRequest>((r) => posted.Add((PutApprenticeshipPaymentSummaryRequestData)r.Data!))
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
            ProviderId = id,
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
