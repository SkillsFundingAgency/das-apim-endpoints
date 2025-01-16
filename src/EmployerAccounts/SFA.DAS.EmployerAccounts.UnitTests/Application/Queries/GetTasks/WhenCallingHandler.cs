using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks;

[TestFixture]
public class WhenCallingHandler
{
    [Test, MoqAutoData]
    public async Task Then_NumberTransferPledgeApplicationsToReview_Should_Match_Api_Response(
        [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLTMApi,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetApplicationsResponse ltmApplicationsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockLTMApi
            .Setup(m => m.Get<GetApplicationsResponse>(It.Is<GetApplicationsRequest>(r =>
                r.SenderAccountId == request.AccountId
                && r.ApplicationStatusFilter == ApplicationStatus.Pending)))
            .ReturnsAsync(ltmApplicationsResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberTransferPledgeApplicationsToReview.Should().Be(ltmApplicationsResponse.TotalItems);
    }

    [Test, MoqAutoData]
    public async Task Then_NumberOfApprenticesToReview_Is_Returned(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetApprenticeshipUpdatesResponse apprenticeshipUpdatesResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCommitmentsApi
            .Setup(m => m.Get<GetApprenticeshipUpdatesResponse>(It.Is<GetPendingApprenticeChangesRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(apprenticeshipUpdatesResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfApprenticesToReview.Should().Be(3);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_NumberOfTransferRequestToReview_When_Valid(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        // Arrange
        foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
        {
            tr.Status = TransferApprovalStatus.Pending;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfTransferRequestToReview.Should().Be(3);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_NumberOfTransferPledgeApplicationsApproved_When_Valid(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        // Arrange
        foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
        {
            tr.Status = TransferApprovalStatus.Approved;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfTransferPledgeApplicationsApproved.Should().Be(3);
        result.SingleApprovedTransferPledgeHashedId.Should().BeNullOrEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_Null_SingleApprovedTransferPledgeHashedId_When_NumberOfTransferPledgeApplicationsApproved_More_Than_One(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        // Arrange
        foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
        {
            tr.Status = TransferApprovalStatus.Approved;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.SingleApprovedTransferPledgeHashedId.Should().BeNullOrEmpty();
    }
    
    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_Null_SingleApprovedTransferPledgeHashedId_When_NumberOfTransferPledgeApplicationsApproved_Is_Zero(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        // Arrange
        foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
        {
            tr.Status = TransferApprovalStatus.Pending;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfTransferPledgeApplicationsApproved.Should().Be(0);
        result.SingleApprovedTransferPledgeHashedId.Should().BeNullOrEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_SingleApprovedTransferPledgeHashedId_When_NumberOfTransferPledgeApplicationsApproved_Is_One(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        transferRequestResponse.TransferRequestSummaryResponse = [transferRequestResponse.TransferRequestSummaryResponse.First()];

        // Arrange
        foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
        {
            tr.Status = TransferApprovalStatus.Approved;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.SingleApprovedTransferPledgeHashedId.Should().Be(transferRequestResponse.TransferRequestSummaryResponse.First().HashedTransferRequestId);
    }

    [Test, MoqAutoData]
    public async Task Then_Only_Returns_Pending_TransferRequests(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        transferRequestResponse.TransferRequestSummaryResponse.Select((cohort, index) =>
        {
            cohort.Status = index == 0 ? TransferApprovalStatus.Approved : TransferApprovalStatus.Pending;
            return cohort;
        }).ToArray();

        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(transferRequestResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        result.NumberOfTransferRequestToReview.Should().Be(2);
    }

    [Test, MoqAutoData]
    public async Task When_TransferRequests_Are_Null_Return_Zero(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCommitmentsApi
            .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r =>
                r.AccountId == request.AccountId && r.Originator == TransferType.AsSender)))
            .ReturnsAsync(new GetTransferRequestSummaryResponse());

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfTransferRequestToReview.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_NumberOfPendingTransferConnections(
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> _financeApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        List<GetTransferConnectionsResponse.TransferConnection> transferConnectionsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        _financeApiClient
            .Setup(m => m.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
                It.Is<GetTransferConnectionsRequest>(
                    r => r.AccountId == request.AccountId && r.Status == TransferConnectionInvitationStatus.Pending
                )))
            .ReturnsAsync(transferConnectionsResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.NumberOfPendingTransferConnections.Should().Be(transferConnectionsResponse.Count);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_Tasks_Returns_NumberOfCohortsReadyToReview_Where_Valid(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetCohortsResponse cohortsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        // Arrange
        foreach (var cohort in cohortsResponse.Cohorts)
        {
            cohort.WithParty = Party.Employer;
            cohort.IsDraft = false;
        }

        mockCommitmentsApi
            .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(cohortsResponse);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        result.NumberOfCohortsReadyToReview.Should().Be(3);
    }


    [Test, MoqAutoData]
    public async Task Then_Only_Returns_Pending_Employer_Cohorts(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetCohortsResponse cohortsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        cohortsResponse.Cohorts.Select((cohort, index) =>
        {
            cohort.WithParty = index == 0 ? Party.Provider : Party.Employer;
            cohort.IsDraft = index == 0;
            return cohort;
        }).ToArray();

        mockCommitmentsApi
            .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(cohortsResponse);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        result.NumberOfCohortsReadyToReview.Should().Be(2);
    }


    [Test, MoqAutoData]
    public async Task When_Cohorts_Are_Null_Return_Zero(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCommitmentsApi
            .Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.NumberOfCohortsReadyToReview.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_ShowLevyDeclarationTask_Is_True_If_In_DateRange_And_Levy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 18));
        accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy;
        mockAccountApi
            .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(accountResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.ShowLevyDeclarationTask.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_ShowLevyDeclarationTask_Is_False_If_In_DateRange_And_NonLevy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 18));
        accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.NonLevy;
        mockAccountApi
            .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(accountResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.ShowLevyDeclarationTask.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_ShowLevyDeclarationTask_Is_False_If_Out_Of_DateRange_And_Levy(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountApi,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        [Frozen] Mock<ICurrentDateTime> mockCurrentDateTime,
        GetAccountByIdResponse accountResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
    {
        mockCurrentDateTime.Setup(m => m.Now).Returns(new DateTime(2024, 01, 13));
        accountResponse.ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy;
        mockAccountApi
            .Setup(m => m.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(accountResponse);

        mockCommitmentsApi.Setup(m => m.Get<GetCohortsResponse>(It.Is<GetCohortsRequest>(r => r.AccountId == request.AccountId)))
            .ReturnsAsync(new GetCohortsResponse());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        result.ShowLevyDeclarationTask.Should().BeFalse();
    }
}