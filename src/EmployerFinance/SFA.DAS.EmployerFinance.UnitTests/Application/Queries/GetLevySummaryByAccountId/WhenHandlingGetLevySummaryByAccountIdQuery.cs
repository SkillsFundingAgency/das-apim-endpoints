using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevySummaryByAccountId;

[TestFixture]
public class GetLevySummaryByAccountIdQueryHandlerTests
{
    private const int PageItemCount = 100;

    private static void SetupCommitmentsPage(
        Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mock,
        long? accountId,
        long? transferSenderId,
        int pageNumber,
        GetCommittedLearnersCostByAccountIdResponse response)
    {
        mock.Setup(c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r =>
                    r.AccountId == accountId &&
                    r.TransferSenderId == transferSenderId &&
                    r.PageNumber == pageNumber &&
                    r.PageItemCount == PageItemCount)))
            .ReturnsAsync(response);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Mapped_Result_When_All_Data_Fits_In_Single_Page(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange
        committedLearnersResponse.TotalApprenticeshipsFound = 10;
        committedTransferOutResponse.TotalApprenticeshipsFound = 5;
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, null, query.AccountId, 1, committedTransferOutResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CurrentLevyFunds.Should().Be(levySummaryResponse.CurrentLevyFunds);
        result.TotalLevyDeclaredLast12Months.Should().Be(levySummaryResponse.TotalLevyDeclaredLast12Months);
        result.TotalLevySpentLast12Months.Should().Be(levySummaryResponse.TotalLevySpentLast12Months);
        result.TotalLevyExpiredLast12Months.Should().Be(levySummaryResponse.TotalLevyExpiredLast12Months);
        result.TotalCommittedLearnerCosts.Should().Be(
            Convert.ToDecimal(committedLearnersResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice)));
        result.TotalCommittedTransfersCosts.Should().Be(
            Convert.ToDecimal(committedTransferOutResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice)));
    }

    [Test, MoqAutoData]
    public async Task Then_Only_One_Page_Request_Made_When_Total_Within_Page_Size(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange
        committedLearnersResponse.TotalApprenticeshipsFound = PageItemCount;
        committedTransferOutResponse.TotalApprenticeshipsFound = PageItemCount;
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, null, query.AccountId, 1, committedTransferOutResponse);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert — no page 2 request should be made
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r => r.PageNumber > 1)),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Fetches_All_Pages_And_Combines_Results_When_Total_Exceeds_Page_Size(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse pageOneResponse,
        GetCommittedLearnersCostByAccountIdResponse pageTwoResponse,
        GetCommittedLearnersCostByAccountIdResponse transferPageOneResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange — 150 apprenticeships spans two pages
        pageOneResponse.TotalApprenticeshipsFound = 150;
        pageTwoResponse.TotalApprenticeshipsFound = 150;
        transferPageOneResponse.TotalApprenticeshipsFound = 5;

        pageOneResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        pageTwoResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        transferPageOneResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, pageOneResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 2, pageTwoResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, null, query.AccountId, 1, transferPageOneResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert — page 2 was requested
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetCommittedLearnersCostByAccountIdResponse>(
                It.Is<GetCommittedLearnersCostByAccountIdRequest>(r =>
                    r.AccountId == query.AccountId &&
                    r.TransferSenderId == null &&
                    r.PageNumber == 2)),
            Times.Once);

        // Assert — combined cost from both pages
        var expectedCost = Convert.ToDecimal(
            pageOneResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice) +
            pageTwoResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice));

        result.TotalCommittedLearnerCosts.Should().Be(expectedCost);
    }

    [Test, MoqAutoData]
    public async Task Then_Uses_TotalAgreedPrice_And_Does_Not_Fetch_Price_Episodes_When_Change_History_Exists(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange
        committedLearnersResponse.TotalApprenticeshipsFound = committedLearnersResponse.Apprenticeships.Count();
        committedTransferOutResponse.TotalApprenticeshipsFound = committedTransferOutResponse.Apprenticeships.Count();
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = true);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = true);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, null, query.AccountId, 1, committedTransferOutResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetPriceEpisodeResponse>(It.IsAny<GetPriceEpisodeByApprenticeshipIdRequest>()),
            Times.Never);
        result.TotalCommittedLearnerCosts.Should().Be(
            Convert.ToDecimal(committedLearnersResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice)));
        result.TotalCommittedTransfersCosts.Should().Be(
            Convert.ToDecimal(committedTransferOutResponse.Apprenticeships.Sum(a => a.TotalAgreedPrice)));
    }
}