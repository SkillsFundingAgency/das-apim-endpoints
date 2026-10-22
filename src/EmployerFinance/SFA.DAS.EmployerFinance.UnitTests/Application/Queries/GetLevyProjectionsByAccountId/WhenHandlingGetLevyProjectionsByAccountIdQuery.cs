#nullable enable
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionsByAccountId;

[TestFixture]
internal class WhenHandlingGetLevyProjectionsByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Projection_Is_Returned(
        long accountId,
        DateTime lastSubmissionDate,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var now = DateTime.UtcNow;
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);

        // Convert now to HMRC payroll period
        short payrollMonth = now.Month >= 4
            ? (short)(now.Month - 3)
            : (short)(now.Month + 9);

        var startYear = now.Month >= 4 ? now.Year % 100 : (now.Year - 1) % 100;
        var payrollYear = $"{startYear:D2}-{startYear + 1:D2}";

        GetLevySummaryByAccountIdRequest? capturedLevySummaryByAccountIdRequest = null;
        financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .Callback<IGetApiRequest>(r => capturedLevySummaryByAccountIdRequest = r as GetLevySummaryByAccountIdRequest)
            .ReturnsAsync(levySummaryByAccountIdResponse);

        GetLevyDeclarationSummaryByDate? capturedAccountTransactionSummaryByDateRequest = null;
        financeApiClient
            .Setup(x => x.Get<List<LevyDeclaration>>(It.IsAny<GetLevyDeclarationSummaryByDate>()))
            .Callback<IGetApiRequest>(r => capturedAccountTransactionSummaryByDateRequest = r as GetLevyDeclarationSummaryByDate)
            .ReturnsAsync([
                new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m, SubmissionDate = lastSubmissionDate.AddDays(-10) },
                new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 0m, TotalAmount = 500m, SubmissionDate = lastSubmissionDate },
                new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 0m, TotalAmount = 100m, SubmissionDate = lastSubmissionDate.AddDays(-5) },
            ]);
        
        PostEmployerFundingProjectionRequest? capturedEmployerFundingProjectionRequest = null;
        projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => capturedEmployerFundingProjectionRequest = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(new ApiResponse<EstimatesTimeline>(estimatesTimeline, HttpStatusCode.OK, null!));

        List<LevyInMonthSummary> expectedHistoricLevyIn = [new(new DateOnly(now.Year, now.Month, 1), 1100m)];
        
        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        capturedLevySummaryByAccountIdRequest.Should().NotBeNull();
        capturedLevySummaryByAccountIdRequest!.AccountId.Should().Be(request.AccountId);
        
        capturedAccountTransactionSummaryByDateRequest.Should().NotBeNull();
        capturedAccountTransactionSummaryByDateRequest!.AccountId.Should().Be(request.AccountId);
        
        capturedEmployerFundingProjectionRequest.Should().NotBeNull();
        capturedEmployerFundingProjectionRequest!.AccountId.Should().Be(request.AccountId);
        capturedEmployerFundingProjectionRequest!.PostData.Months.Should().Be(6);
        capturedEmployerFundingProjectionRequest!.PostData.HistoricLevyIn.Should().BeEquivalentTo(expectedHistoricLevyIn);

        result.AccountId.Should().Be(request.AccountId);
        result.Summary.Should().BeEquivalentTo(levySummaryByAccountIdResponse);
        result.Summary.TotalCommittedLearnerCosts.Should().Be(estimatesTimeline.TotalEstimatedCommitedCost);
        result.Summary.TotalCommittedTransfersCosts.Should().Be(estimatesTimeline.TotalEstimatedCommitedTransferCost);
        result.Projections.Should().BeEquivalentTo(estimatesTimeline.Projections);
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Handler_Sums_The_Transactions_Before_Projection(
        long accountId,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var now = DateTime.UtcNow;
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);

        // Convert now to HMRC payroll period
        short payrollMonth = now.Month >= 4
            ? (short)(now.Month - 3)
            : (short)(now.Month + 9);

        var startYear = now.Month >= 4 ? now.Year % 100 : (now.Year - 1) % 100;
        var payrollYear = $"{startYear:D2}-{startYear + 1:D2}";

        financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .ReturnsAsync(levySummaryByAccountIdResponse);
        
        financeApiClient
            .Setup(x => x.Get<List<LevyDeclaration>>(It.IsAny<GetLevyDeclarationSummaryByDate>()))
            .ReturnsAsync([
                new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m },
                new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m },
            ]);
        
        PostEmployerFundingProjectionRequest? capturedEmployerFundingProjectionRequest = null;
        projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => capturedEmployerFundingProjectionRequest = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(new ApiResponse<EstimatesTimeline>(estimatesTimeline, HttpStatusCode.OK, null!));

        List<LevyInMonthSummary> expectedHistoricLevyIn = [new(new DateOnly(now.Year, now.Month, 1), 1000m)];
        
        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        capturedEmployerFundingProjectionRequest.Should().NotBeNull();
        capturedEmployerFundingProjectionRequest!.PostData.HistoricLevyIn.Should().BeEquivalentTo(expectedHistoricLevyIn);
    }

    [Test, MoqAutoData]
    public async Task Then_Funds_In_Funds_Out_And_Expired_Levy_Are_Passed_To_The_Projection(
        long accountId,
        List<CalendarPeriodAmount> fundsIn,
        List<CalendarPeriodAmount> fundsOut,
        GetExpiredLevyResponse expiredLevy,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupCommonFinanceCalls(financeApiClient, levySummaryByAccountIdResponse);

        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.Is<GetLevyFundsInRequest>(r => r.AccountId == accountId)))
            .ReturnsAsync(fundsIn);
        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.Is<GetPaymentFundsOutRequest>(r => r.AccountId == accountId)))
            .ReturnsAsync(fundsOut);
        financeApiClient
            .Setup(x => x.Get<GetExpiredLevyResponse>(It.Is<GetExpiredLevyRequest>(r => r.AccountId == accountId)))
            .ReturnsAsync(expiredLevy);

        var capturedRequest = SetupProjection(projectionApiClient, estimatesTimeline);

        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        capturedRequest.Value.Should().NotBeNull();
        capturedRequest.Value!.PostData.LevyFundsInForExpiry.Should().BeEquivalentTo(fundsIn);
        capturedRequest.Value.PostData.PaymentFundsOutForExpiry.Should().BeEquivalentTo(fundsOut);
        capturedRequest.Value.PostData.LongTermExpiredFunds.Should().BeEquivalentTo(expiredLevy.LongTerm);
        capturedRequest.Value.PostData.ShortTermExpiredFunds.Should().BeEquivalentTo(expiredLevy.ShortTerm);
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_Lists_Are_Passed_To_The_Projection_When_The_Finance_Api_Returns_No_Data(
        long accountId,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupCommonFinanceCalls(financeApiClient, levySummaryByAccountIdResponse);

        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetLevyFundsInRequest>()))
            .ReturnsAsync([]);
        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetPaymentFundsOutRequest>()))
            .ReturnsAsync([]);
        financeApiClient
            .Setup(x => x.Get<GetExpiredLevyResponse>(It.IsAny<GetExpiredLevyRequest>()))
            .ReturnsAsync(new GetExpiredLevyResponse());

        var capturedRequest = SetupProjection(projectionApiClient, estimatesTimeline);

        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        capturedRequest.Value.Should().NotBeNull();
        capturedRequest.Value!.PostData.LevyFundsInForExpiry.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.PaymentFundsOutForExpiry.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.LongTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.ShortTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_Lists_Are_Passed_To_The_Projection_When_The_Finance_Api_Returns_Null(
        long accountId,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupCommonFinanceCalls(financeApiClient, levySummaryByAccountIdResponse);

        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetLevyFundsInRequest>()))
            .ReturnsAsync((List<CalendarPeriodAmount>)null!);
        financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetPaymentFundsOutRequest>()))
            .ReturnsAsync((List<CalendarPeriodAmount>)null!);
        financeApiClient
            .Setup(x => x.Get<GetExpiredLevyResponse>(It.IsAny<GetExpiredLevyRequest>()))
            .ReturnsAsync((GetExpiredLevyResponse)null!);

        var capturedRequest = SetupProjection(projectionApiClient, estimatesTimeline);

        // act
        await sut.Handle(request, CancellationToken.None);

        // assert
        capturedRequest.Value.Should().NotBeNull();
        capturedRequest.Value!.PostData.LevyFundsInForExpiry.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.PaymentFundsOutForExpiry.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.LongTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
        capturedRequest.Value.PostData.ShortTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_The_Latest_Declaration_Date_Ignores_Non_Positive_Declarations(
        long accountId,
        DateTime lastSubmissionDate,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupCommonFinanceCalls(financeApiClient, levySummaryByAccountIdResponse);
        SetupProjection(projectionApiClient, estimatesTimeline);

        financeApiClient
            .Setup(x => x.Get<List<LevyDeclaration>>(It.IsAny<GetLevyDeclarationSummaryByDate>()))
            .ReturnsAsync([
                new LevyDeclaration { TotalAmount = 1000m, SubmissionDate = lastSubmissionDate },
                new LevyDeclaration { TotalAmount = 0m, SubmissionDate = lastSubmissionDate.AddDays(1) },
                new LevyDeclaration { TotalAmount = -200m, SubmissionDate = lastSubmissionDate.AddDays(2) },
            ]);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Latest_Declaration_Date_Is_MinValue_When_There_Are_No_Declarations(
        long accountId,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse,
        EstimatesTimeline estimatesTimeline,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler sut)
    {
        // arrange
        var request = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupCommonFinanceCalls(financeApiClient, levySummaryByAccountIdResponse);
        SetupProjection(projectionApiClient, estimatesTimeline);

        // act
        var result = await sut.Handle(request, CancellationToken.None);

        // assert
        result.LatestLevyDeclarationInDate.Should().Be(DateTime.MinValue);
    }

    private static void SetupCommonFinanceCalls(
        Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        GetLevySummaryByAccountIdResponse levySummaryByAccountIdResponse)
    {
        financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .ReturnsAsync(levySummaryByAccountIdResponse);
        financeApiClient
            .Setup(x => x.Get<List<LevyDeclaration>>(It.IsAny<GetLevyDeclarationSummaryByDate>()))
            .ReturnsAsync([]);
    }

    private static StrongBox<PostEmployerFundingProjectionRequest?> SetupProjection(
        Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> projectionApiClient,
        EstimatesTimeline estimatesTimeline)
    {
        var captured = new StrongBox<PostEmployerFundingProjectionRequest?>();
        projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => captured.Value = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(new ApiResponse<EstimatesTimeline>(estimatesTimeline, HttpStatusCode.OK, null!));
        return captured;
    }
}
