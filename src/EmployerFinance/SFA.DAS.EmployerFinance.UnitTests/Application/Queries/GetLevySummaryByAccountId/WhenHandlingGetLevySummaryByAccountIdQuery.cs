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
        long accountId,
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
        committedLearnersResponse.TotalApprenticeships = 10;
        committedTransferOutResponse.TotalApprenticeships = 5;
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CurrentLevyFunds.Should().Be(levySummaryResponse.CurrentLevyFunds);
        result.TotalLevyDeclaredLast12Months.Should().Be(levySummaryResponse.TotalLevyDeclaredLast12Months);
        result.TotalLevySpentLast12Months.Should().Be(levySummaryResponse.TotalLevySpentLast12Months);
        result.TotalLevyExpiredLast12Months.Should().Be(levySummaryResponse.TotalLevyExpiredLast12Months);
        result.TotalCommittedLearnerCosts.Should().Be(
            Convert.ToDecimal(committedLearnersResponse.Apprenticeships.Sum(a => a.Cost)));
        result.TotalCommittedTransfersCosts.Should().Be(
            Convert.ToDecimal(committedTransferOutResponse.Apprenticeships.Sum(a => a.Cost)));
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
        committedLearnersResponse.TotalApprenticeships = PageItemCount;
        committedTransferOutResponse.TotalApprenticeships = PageItemCount;
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

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
        pageOneResponse.TotalApprenticeships = 150;
        pageTwoResponse.TotalApprenticeships = 150;
        transferPageOneResponse.TotalApprenticeships = 5;

        pageOneResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        pageTwoResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        transferPageOneResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, pageOneResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 2, pageTwoResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, transferPageOneResponse);

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
            pageOneResponse.Apprenticeships.Sum(a => a.Cost) +
            pageTwoResponse.Apprenticeships.Sum(a => a.Cost));

        result.TotalCommittedLearnerCosts.Should().Be(expectedCost);
    }

    [Test, MoqAutoData]
    public async Task Then_Fetches_Price_Episodes_For_Apprenticeships_With_Change_History(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        GetPriceEpisodeResponse priceEpisodeResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange — all learners have change history
        committedLearnersResponse.TotalApprenticeships = committedLearnersResponse.Apprenticeships.Count();
        committedTransferOutResponse.TotalApprenticeships = committedTransferOutResponse.Apprenticeships.Count();
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = true);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

        mockCommitmentsV2ApiClient
            .Setup(c => c.Get<GetPriceEpisodeResponse>(It.IsAny<GetPriceEpisodeByApprenticeshipIdRequest>()))
            .ReturnsAsync(priceEpisodeResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert — one price episode call per apprenticeship with change history
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetPriceEpisodeResponse>(It.IsAny<GetPriceEpisodeByApprenticeshipIdRequest>()),
            Times.Exactly(committedLearnersResponse.Apprenticeships.Count()));

        var expectedCost = Convert.ToDecimal(
            committedLearnersResponse.Apprenticeships.Count() *
            priceEpisodeResponse.PriceEpisodes.Sum(pe => pe.Cost));

        result.TotalCommittedLearnerCosts.Should().Be(expectedCost);
    }

    [Test, MoqAutoData]
    public async Task Then_Does_Not_Fetch_Price_Episodes_When_No_Apprenticeships_Have_Change_History(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange
        committedLearnersResponse.TotalApprenticeships = committedLearnersResponse.Apprenticeships.Count();
        committedTransferOutResponse.TotalApprenticeships = committedTransferOutResponse.Apprenticeships.Count();
        committedLearnersResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetPriceEpisodeResponse>(It.IsAny<GetPriceEpisodeByApprenticeshipIdRequest>()),
            Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Fetches_Price_Episodes_Only_For_Apprenticeships_With_Change_History(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        GetPriceEpisodeResponse priceEpisodeResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange — only first apprenticeship has change history
        var apprenticeships = committedLearnersResponse.Apprenticeships.ToList();
        apprenticeships[0].HasChangeHistory = true;
        apprenticeships.Skip(1).ToList().ForEach(a => a.HasChangeHistory = false);

        committedLearnersResponse.TotalApprenticeships = apprenticeships.Count;
        committedTransferOutResponse.TotalApprenticeships = committedTransferOutResponse.Apprenticeships.Count();
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

        mockCommitmentsV2ApiClient
            .Setup(c => c.Get<GetPriceEpisodeResponse>(
                It.Is<GetPriceEpisodeByApprenticeshipIdRequest>(r => r.ApprenticeshipId == apprenticeships[0].Id)))
            .ReturnsAsync(priceEpisodeResponse);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert — called only once for the qualifying apprenticeship
        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetPriceEpisodeResponse>(It.IsAny<GetPriceEpisodeByApprenticeshipIdRequest>()),
            Times.Once);

        mockCommitmentsV2ApiClient.Verify(
            c => c.Get<GetPriceEpisodeResponse>(
                It.Is<GetPriceEpisodeByApprenticeshipIdRequest>(r =>
                    r.ApprenticeshipId == apprenticeships[0].Id)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Price_Episode_Cost_Overwrites_Apprenticeship_Cost_When_Change_History_Exists(
        GetLevySummaryByAccountIdQuery query,
        GetLevySummaryByAccountIdResponse levySummaryResponse,
        GetCommittedLearnersCostByAccountIdResponse committedLearnersResponse,
        GetCommittedLearnersCostByAccountIdResponse committedTransferOutResponse,
        GetPriceEpisodeResponse priceEpisodeResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiClient,
        [Greedy] GetLevySummaryByAccountIdQueryHandler handler)
    {
        // Arrange — single apprenticeship with change history
        var apprenticeship = committedLearnersResponse.Apprenticeships.First();
        apprenticeship.HasChangeHistory = true;
        committedLearnersResponse.Apprenticeships = new List<GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse> { apprenticeship };
        committedLearnersResponse.TotalApprenticeships = 1;
        committedTransferOutResponse.TotalApprenticeships = committedTransferOutResponse.Apprenticeships.Count();
        committedTransferOutResponse.Apprenticeships.ToList().ForEach(a => a.HasChangeHistory = false);

        mockFinanceApiClient
            .Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == query.AccountId)))
            .ReturnsAsync(levySummaryResponse);

        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, null, 1, committedLearnersResponse);
        SetupCommitmentsPage(mockCommitmentsV2ApiClient, query.AccountId, query.AccountId, 1, committedTransferOutResponse);

        mockCommitmentsV2ApiClient
            .Setup(c => c.Get<GetPriceEpisodeResponse>(
                It.Is<GetPriceEpisodeByApprenticeshipIdRequest>(r => r.ApprenticeshipId == apprenticeship.Id)))
            .ReturnsAsync(priceEpisodeResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert — cost comes from price episodes, not the original apprenticeship value
        var expectedCost = Convert.ToDecimal(priceEpisodeResponse.PriceEpisodes.Sum(pe => pe.Cost));
        result.TotalCommittedLearnerCosts.Should().Be(expectedCost);
    }
}