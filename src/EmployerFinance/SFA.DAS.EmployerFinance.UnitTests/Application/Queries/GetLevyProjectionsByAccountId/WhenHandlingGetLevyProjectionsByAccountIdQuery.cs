#nullable enable
using AutoFixture;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionsByAccountId;

internal class WhenHandlingGetLevyProjectionsByAccountIdQuery
{
    private const long AccountId = 12345;

    private static readonly List<string> DefaultEmprefs = ["123/AB456", "456/CD789"];
    private const string HashOfDefaultEmprefs = "98B97C29FAAA5FF3";
    
    private readonly Fixture _fixture = new();

    private Mock<IFinanceApiClient<FinanceApiConfiguration>> _financeApiClient = null!;
    private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClient = null!;
    private Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> _projectionApiClient = null!;
    private Mock<ICacheStorageService> _cacheStorageService = null!;
    private GetLevyProjectionsByAccountIdQueryHandler _sut = null!;

    private GetLevySummaryByAccountIdResponse _levySummary = null!;
    private EstimatesTimeline _estimatesTimeline = null!;
    private PostEmployerFundingProjectionRequest? _projectionRequest;
    private PostLevyDeclarationSummaryByDate? _declarationsRequest;

    [SetUp]
    public void Arrange()
    {
        _financeApiClient = new Mock<IFinanceApiClient<FinanceApiConfiguration>>();
        _accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        _projectionApiClient = new Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>>();
        _cacheStorageService = new Mock<ICacheStorageService>();
        _sut = new GetLevyProjectionsByAccountIdQueryHandler(
            _financeApiClient.Object,
            _accountsApiClient.Object,
            _projectionApiClient.Object,
            _cacheStorageService.Object,
            Mock.Of<ILogger<GetLevyProjectionsByAccountIdQueryHandler>>());

        _levySummary = _fixture.Create<GetLevySummaryByAccountIdResponse>();
        _estimatesTimeline = _fixture.Create<EstimatesTimeline>();
        _projectionRequest = null;
        _declarationsRequest = null;

        SetupPayeSchemes(DefaultEmprefs);
        SetupLatestIds(new GetLatestTransactionTypeTransactionIdsForAccountResponse { LatestDeclarationId = 121, LatestPaymentId = 115, LatestExpiryId = 100 });
        SetupFundsData([], [], new GetExpiredLevyResponse());
        SetupDeclarations([]);
        SetupCacheMisses();

        _financeApiClient
            .Setup(x => x.Get<GetLevySummaryByAccountIdResponse>(It.IsAny<GetLevySummaryByAccountIdRequest>()))
            .ReturnsAsync(() => _levySummary);
        _projectionApiClient
            .Setup(x => x.PostWithResponseCode<EstimatesTimeline>(It.IsAny<PostEmployerFundingProjectionRequest>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => _projectionRequest = r as PostEmployerFundingProjectionRequest)
            .ReturnsAsync(() => new ApiResponse<EstimatesTimeline>(_estimatesTimeline, HttpStatusCode.OK, null!));
    }

    [Test]
    public async Task Then_The_Projection_Is_Returned()
    {
        // arrange
        var now = DateTime.UtcNow;
        var lastSubmissionDate = new DateTime(2026, 1, 15);
        var (payrollMonth, payrollYear) = ToPayrollPeriod(now);
        SetupDeclarations([
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m, SubmissionDate = lastSubmissionDate.AddDays(-10) },
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 0m, TotalAmount = 500m, SubmissionDate = lastSubmissionDate },
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 0m, TotalAmount = 100m, SubmissionDate = lastSubmissionDate.AddDays(-5) },
        ]);

        // act
        var result = await Handle();

        // assert
        _financeApiClient.Verify(x => x.Get<GetLevySummaryByAccountIdResponse>(It.Is<GetLevySummaryByAccountIdRequest>(r => r.AccountId == AccountId)), Times.Once);
        _declarationsRequest!.AccountId.Should().Be(AccountId);

        _projectionRequest!.AccountId.Should().Be(AccountId);
        _projectionRequest.PostData.Months.Should().Be(6);
        _projectionRequest.PostData.HistoricLevyIn.Should().BeEquivalentTo(
            new List<LevyInMonthSummary> { new(new DateOnly(now.Year, now.Month, 1).AddMonths(1), 1100m) });

        result.AccountId.Should().Be(AccountId);
        result.Summary.Should().BeEquivalentTo(_levySummary);
        result.Summary.TotalCommittedLearnerCosts.Should().Be(_estimatesTimeline.TotalEstimatedCommitedCost);
        result.Summary.TotalCommittedTransfersCosts.Should().Be(_estimatesTimeline.TotalEstimatedCommitedTransferCost);
        result.Projections.Should().BeEquivalentTo(_estimatesTimeline.Projections);
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }

    [Test]
    public async Task Then_The_Handler_Sums_The_Transactions_Before_Projection()
    {
        // arrange
        var now = DateTime.UtcNow;
        var (payrollMonth, payrollYear) = ToPayrollPeriod(now);
        SetupDeclarations([
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m },
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 500m, TotalAmount = 1000m },
        ]);

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.HistoricLevyIn.Should().BeEquivalentTo(
            new List<LevyInMonthSummary> { new(new DateOnly(now.Year, now.Month, 1).AddMonths(1), 1000m) });
    }

    [Test]
    public async Task Then_The_Earliest_Payroll_Month_Requested_Is_Projected_As_Levy_In_A_Year_Ago()
    {
        // arrange
        var now = DateTime.UtcNow;
        var (payrollMonth, payrollYear) = ToPayrollPeriod(new DateTime(now.Year, now.Month, 1).AddMonths(-13));
        SetupDeclarations([
            new LevyDeclaration { PayrollMonth = payrollMonth, PayrollYear = payrollYear, TopUp = 100m, TotalAmount = 1100m },
        ]);

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.HistoricLevyIn.Should().BeEquivalentTo(
            new List<LevyInMonthSummary> { new(new DateOnly(now.Year, now.Month, 1).AddMonths(-12), 1000m) });
    }

    [Test]
    public async Task Then_Declarations_Are_Requested_From_Thirteen_Months_Ago_To_The_End_Of_The_Current_Month()
    {
        // arrange
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        // act
        await Handle();

        // assert
        // The levy for a payroll month arrives the month after, so an extra month is needed to cover a full year of levy in
        _declarationsRequest!.FromDate.Should().Be(startOfMonth.AddMonths(-13));
        _declarationsRequest.ToDate.Should().Be(startOfMonth.AddMonths(1).AddTicks(-1));
    }

    [Test]
    public async Task Then_The_Latest_Declaration_Date_Ignores_Non_Positive_Declarations()
    {
        // arrange
        var lastSubmissionDate = new DateTime(2026, 1, 15);
        SetupDeclarations([
            new LevyDeclaration { TotalAmount = 1000m, SubmissionDate = lastSubmissionDate },
            new LevyDeclaration { TotalAmount = 0m, SubmissionDate = lastSubmissionDate.AddDays(1) },
            new LevyDeclaration { TotalAmount = -200m, SubmissionDate = lastSubmissionDate.AddDays(2) },
        ]);

        // act
        var result = await Handle();

        // assert
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }

    [Test]
    public async Task Then_The_Latest_Declaration_Date_Is_MinValue_When_There_Are_No_Declarations()
    {
        // act
        var result = await Handle();

        // assert
        result.LatestLevyDeclarationInDate.Should().Be(DateTime.MinValue);
    }

    [Test]
    public async Task Then_Funds_In_Funds_Out_And_Expired_Levy_Are_Passed_To_The_Projection()
    {
        // arrange
        var (fundsIn, fundsOut, expiredLevy) = SetupRandomFundsData();

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.LevyFundsInForExpiry.Should().BeEquivalentTo(fundsIn);
        _projectionRequest.PostData.PaymentFundsOutForExpiry.Should().BeEquivalentTo(fundsOut);
        _projectionRequest.PostData.LongTermExpiredFunds.Should().BeEquivalentTo(expiredLevy.LongTerm);
        _projectionRequest.PostData.ShortTermExpiredFunds.Should().BeEquivalentTo(expiredLevy.ShortTerm);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Then_Empty_Lists_Are_Passed_To_The_Projection_When_The_Finance_Api_Returns_No_Data(bool returnsNull)
    {
        // arrange
        if (returnsNull)
            SetupFundsData(null, null, null);

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.LevyFundsInForExpiry.Should().NotBeNull().And.BeEmpty();
        _projectionRequest.PostData.PaymentFundsOutForExpiry.Should().NotBeNull().And.BeEmpty();
        _projectionRequest.PostData.LongTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
        _projectionRequest.PostData.ShortTermExpiredFunds.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    public async Task Then_Last_Months_Collection_Period_Payments_Are_Passed_To_The_Projection_As_The_Current_Payments()
    {
        // arrange
        // Payments land the month after their collection period, so this month's payments are last month's collection
        var collectionPeriod = DateTime.UtcNow.AddMonths(-1);
        var currentPayments = _fixture.Create<CollectionPeriodPayments>();
        _financeApiClient
            .Setup(x => x.Get<CollectionPeriodPayments>(It.Is<GetCollectionPeriodPaymentsRequest>(r =>
                r.AccountId == AccountId && r.Year == collectionPeriod.Year && r.Month == collectionPeriod.Month)))
            .ReturnsAsync(currentPayments);

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.CurrentPayments.Should().BeSameAs(currentPayments);
    }

    [Test]
    public async Task Then_No_Current_Payments_Are_Passed_To_The_Projection_When_The_Finance_Api_Returns_None()
    {
        // arrange
        _financeApiClient
            .Setup(x => x.Get<CollectionPeriodPayments>(It.IsAny<GetCollectionPeriodPaymentsRequest>()))
            .ReturnsAsync((CollectionPeriodPayments)null!);

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.CurrentPayments.Should().BeNull();
    }

    [Test]
    public async Task Then_Data_Is_Cached_For_Seven_Days_Keyed_On_The_Latest_Transaction_Ids()
    {
        // arrange
        var (fundsIn, fundsOut, expiredLevy) = SetupRandomFundsData();
        List<LevyDeclaration> declarations = [new LevyDeclaration { PayrollMonth = 1, PayrollYear = "25-26", TotalAmount = 100m }];
        SetupDeclarations(declarations);

        // act
        await Handle();

        // assert
        _cacheStorageService.Verify(x => x.SaveToCache($"GetLevyFundsIn_{AccountId}_121", fundsIn, TimeSpan.FromDays(7), null), Times.Once);
        _cacheStorageService.Verify(x => x.SaveToCache($"GetPaymentFundsOut_{AccountId}_115", fundsOut, TimeSpan.FromDays(7), null), Times.Once);
        _cacheStorageService.Verify(x => x.SaveToCache($"GetExpiredLevy_{AccountId}_100", expiredLevy, TimeSpan.FromDays(7), null), Times.Once);
        _cacheStorageService.Verify(x => x.SaveToCache(
            $"GetLevyDeclarationSummaryByDate_{AccountId}_121_{HashOfDefaultEmprefs}", declarations, TimeSpan.FromDays(7), null), Times.Once);
        _cacheStorageService.Verify(x => x.SaveToCache(
            $"LatestTransactionTypeTransactionIds_{AccountId}", It.Is<GetLatestTransactionTypeTransactionIdsForAccountResponse>(l => l.LatestDeclarationId == 121),
            TimeSpan.FromDays(7), null), Times.Once);
    }

    [Test]
    public async Task Then_Cached_Data_Is_Used_When_There_Are_No_New_Transactions()
    {
        // arrange
        var (fundsIn, fundsOut, expiredLevy) = SetupCachedData("121", "115", "100");

        // act
        await Handle();

        // assert
        VerifyCachedDataNotFetched();
        _projectionRequest!.PostData.LevyFundsInForExpiry.Should().BeEquivalentTo(fundsIn);
        _projectionRequest.PostData.PaymentFundsOutForExpiry.Should().BeEquivalentTo(fundsOut);
        _projectionRequest.PostData.LongTermExpiredFunds.Should().BeEquivalentTo(expiredLevy.LongTerm);
    }

    [Test]
    public async Task Then_None_Is_Used_In_The_Cache_Key_When_The_Account_Has_No_Transactions()
    {
        // arrange
        SetupLatestIds(new GetLatestTransactionTypeTransactionIdsForAccountResponse());

        // act
        await Handle();

        // assert
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<CalendarPeriodAmount>>($"GetLevyFundsIn_{AccountId}_none"), Times.Once);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<CalendarPeriodAmount>>($"GetPaymentFundsOut_{AccountId}_none"), Times.Once);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<GetExpiredLevyResponse>($"GetExpiredLevy_{AccountId}_none"), Times.Once);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<LevyDeclaration>>($"GetLevyDeclarationSummaryByDate_{AccountId}_none_{HashOfDefaultEmprefs}"), Times.Once);
    }

    [Test]
    public async Task Then_The_Last_Known_Ids_Are_Used_When_The_Latest_Ids_Cannot_Be_Retrieved()
    {
        // arrange
        _financeApiClient
            .Setup(x => x.Get<GetLatestTransactionTypeTransactionIdsForAccountResponse>(It.IsAny<GetLatestTransactionTypeTransactionIdsForAccountRequest>()))
            .ThrowsAsync(new HttpRequestException());
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetLatestTransactionTypeTransactionIdsForAccountResponse>($"LatestTransactionTypeTransactionIds_{AccountId}"))
            .ReturnsAsync(new GetLatestTransactionTypeTransactionIdsForAccountResponse { LatestDeclarationId = 98, LatestPaymentId = 95, LatestExpiryId = null });
        var (fundsIn, _, _) = SetupCachedData("98", "95", "none");

        // act
        await Handle();

        // assert
        VerifyCachedDataNotFetched();
        _projectionRequest!.PostData.LevyFundsInForExpiry.Should().BeEquivalentTo(fundsIn);
        _cacheStorageService.Verify(x => x.SaveToCache(
            It.IsAny<string>(), It.IsAny<GetLatestTransactionTypeTransactionIdsForAccountResponse>(), It.IsAny<TimeSpan>(), null), Times.Never);
    }

    [Test]
    public async Task Then_The_Data_Is_Fetched_When_The_Latest_Ids_Cannot_Be_Retrieved_And_There_Are_No_Last_Known_Ids()
    {
        // arrange
        SetupLatestIds(null);
        var (fundsIn, _, _) = SetupRandomFundsData();

        // act
        await Handle();

        // assert
        _projectionRequest!.PostData.LevyFundsInForExpiry.Should().BeEquivalentTo(fundsIn);
        _cacheStorageService.Verify(x => x.SaveToCache($"GetLevyFundsIn_{AccountId}_none", fundsIn, TimeSpan.FromDays(7), null), Times.Once);
    }

    [Test]
    public async Task Then_The_Accounts_Paye_Schemes_Are_Sent_With_The_Declarations_Request_And_Not_Cached()
    {
        // act
        await Handle();

        // assert
        _declarationsRequest!.Emprefs.Should().BeEquivalentTo(DefaultEmprefs);
        _declarationsRequest.Data.Should().BeEquivalentTo(DefaultEmprefs);
        _cacheStorageService.Verify(x => x.SaveToCache(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TimeSpan>(), null), Times.Never);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<string>>(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Then_The_Declarations_Cache_Key_Changes_When_A_Paye_Scheme_Is_Added_But_Not_When_The_Order_Changes()
    {
        // act
        SetupPayeSchemes(["456/CD789", "123/AB456"]);
        await Handle();
        SetupPayeSchemes(["123/AB456", "456/CD789", "789/EF012"]);
        await Handle();

        // assert
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<LevyDeclaration>>(
            $"GetLevyDeclarationSummaryByDate_{AccountId}_121_{HashOfDefaultEmprefs}"), Times.Once);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<LevyDeclaration>>(
            $"GetLevyDeclarationSummaryByDate_{AccountId}_121_66DAAF66F1E9F5AC"), Times.Once);
    }

    [TestCase(HttpStatusCode.OK)]
    [TestCase(HttpStatusCode.NotFound)]
    public async Task Then_No_Historic_Levy_Is_Projected_When_The_Account_Has_No_Paye_Schemes(HttpStatusCode payeSchemesStatusCode)
    {
        // arrange
        SetupPayeSchemesResponse(payeSchemesStatusCode, payeSchemesStatusCode == HttpStatusCode.OK ? [] : null);

        // act
        var result = await Handle();

        // assert
        _projectionRequest!.PostData.HistoricLevyIn.Should().BeEmpty();
        result.LatestLevyDeclarationInDate.Should().Be(DateTime.MinValue);
        _financeApiClient.Verify(x => x.PostWithResponseCode<List<LevyDeclaration>>(It.IsAny<PostLevyDeclarationSummaryByDate>(), It.IsAny<bool>()), Times.Never);
        _cacheStorageService.Verify(x => x.RetrieveFromCache<List<LevyDeclaration>>(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Then_The_Request_Fails_When_The_Paye_Schemes_Cannot_Be_Retrieved()
    {
        // arrange
        SetupPayeSchemesResponse(HttpStatusCode.InternalServerError, null);

        // act
        var act = Handle;

        // assert
        await act.Should().ThrowAsync<Exception>();
    }

    private Task<GetLevyProjectionsByAccountIdQueryResult> Handle() =>
        _sut.Handle(new GetLevyProjectionsByAccountIdQuery(AccountId), CancellationToken.None);

    // HMRC payroll months run April (1) to March (12), in tax years written as e.g. "25-26"
    private static (short Month, string Year) ToPayrollPeriod(DateTime date)
    {
        var month = (short)(date.Month >= 4 ? date.Month - 3 : date.Month + 9);
        var startYear = date.Month >= 4 ? date.Year % 100 : (date.Year - 1) % 100;
        return (month, $"{startYear:D2}-{startYear + 1:D2}");
    }

    private void SetupPayeSchemes(List<string> emprefs) =>
        SetupPayeSchemesResponse(HttpStatusCode.OK, emprefs.Select(e => new Resource { Id = e, Href = $"/api/accounts/{AccountId}/payeschemes/{e}" }).ToList());

    private void SetupPayeSchemesResponse(HttpStatusCode statusCode, List<Resource>? payeSchemes)
    {
        _accountsApiClient
            .Setup(x => x.GetWithResponseCode<List<Resource>>(It.Is<GetAccountPayeSchemesRequest>(r => r.GetUrl == $"api/accounts/{AccountId}/payeschemes")))
            .ReturnsAsync(new ApiResponse<List<Resource>>(payeSchemes!, statusCode, string.Empty));
    }

    private void SetupLatestIds(GetLatestTransactionTypeTransactionIdsForAccountResponse? latestIds)
    {
        _financeApiClient
            .Setup(x => x.Get<GetLatestTransactionTypeTransactionIdsForAccountResponse>(It.Is<GetLatestTransactionTypeTransactionIdsForAccountRequest>(r => r.AccountId == AccountId)))
            .ReturnsAsync(latestIds!);
    }

    private void SetupDeclarations(List<LevyDeclaration> declarations)
    {
        _financeApiClient
            .Setup(x => x.PostWithResponseCode<List<LevyDeclaration>>(It.IsAny<PostLevyDeclarationSummaryByDate>(), true))
            .Callback<IPostApiRequest, bool>((r, _) => _declarationsRequest = r as PostLevyDeclarationSummaryByDate)
            .ReturnsAsync(new ApiResponse<List<LevyDeclaration>>(declarations, HttpStatusCode.OK, null!));
    }

    private void SetupFundsData(List<CalendarPeriodAmount>? fundsIn, List<CalendarPeriodAmount>? fundsOut, GetExpiredLevyResponse? expiredLevy)
    {
        _financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.Is<GetLevyFundsInRequest>(r => r.AccountId == AccountId)))
            .ReturnsAsync(fundsIn!);
        _financeApiClient
            .Setup(x => x.Get<List<CalendarPeriodAmount>>(It.Is<GetPaymentFundsOutRequest>(r => r.AccountId == AccountId)))
            .ReturnsAsync(fundsOut!);
        _financeApiClient
            .Setup(x => x.Get<GetExpiredLevyResponse>(It.Is<GetExpiredLevyRequest>(r => r.AccountId == AccountId)))
            .ReturnsAsync(expiredLevy!);
    }

    private (List<CalendarPeriodAmount> FundsIn, List<CalendarPeriodAmount> FundsOut, GetExpiredLevyResponse ExpiredLevy) SetupRandomFundsData()
    {
        var fundsIn = _fixture.CreateMany<CalendarPeriodAmount>().ToList();
        var fundsOut = _fixture.CreateMany<CalendarPeriodAmount>().ToList();
        var expiredLevy = _fixture.Create<GetExpiredLevyResponse>();
        SetupFundsData(fundsIn, fundsOut, expiredLevy);
        return (fundsIn, fundsOut, expiredLevy);
    }

    private void SetupCacheMisses()
    {
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<CalendarPeriodAmount>>(It.IsAny<string>()))
            .ReturnsAsync((List<CalendarPeriodAmount>)null!);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetExpiredLevyResponse>(It.IsAny<string>()))
            .ReturnsAsync((GetExpiredLevyResponse)null!);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<LevyDeclaration>>(It.IsAny<string>()))
            .ReturnsAsync((List<LevyDeclaration>)null!);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetLatestTransactionTypeTransactionIdsForAccountResponse>(It.IsAny<string>()))
            .ReturnsAsync((GetLatestTransactionTypeTransactionIdsForAccountResponse)null!);
    }

    private (List<CalendarPeriodAmount> FundsIn, List<CalendarPeriodAmount> FundsOut, GetExpiredLevyResponse ExpiredLevy) SetupCachedData(
        string declarationKeySegment,
        string paymentKeySegment,
        string expiryKeySegment)
    {
        var fundsIn = _fixture.CreateMany<CalendarPeriodAmount>().ToList();
        var fundsOut = _fixture.CreateMany<CalendarPeriodAmount>().ToList();
        var expiredLevy = _fixture.Create<GetExpiredLevyResponse>();

        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<CalendarPeriodAmount>>($"GetLevyFundsIn_{AccountId}_{declarationKeySegment}"))
            .ReturnsAsync(fundsIn);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<CalendarPeriodAmount>>($"GetPaymentFundsOut_{AccountId}_{paymentKeySegment}"))
            .ReturnsAsync(fundsOut);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<GetExpiredLevyResponse>($"GetExpiredLevy_{AccountId}_{expiryKeySegment}"))
            .ReturnsAsync(expiredLevy);
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<LevyDeclaration>>($"GetLevyDeclarationSummaryByDate_{AccountId}_{declarationKeySegment}_{HashOfDefaultEmprefs}"))
            .ReturnsAsync([]);

        return (fundsIn, fundsOut, expiredLevy);
    }

    private void VerifyCachedDataNotFetched()
    {
        _financeApiClient.Verify(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetLevyFundsInRequest>()), Times.Never);
        _financeApiClient.Verify(x => x.Get<List<CalendarPeriodAmount>>(It.IsAny<GetPaymentFundsOutRequest>()), Times.Never);
        _financeApiClient.Verify(x => x.Get<GetExpiredLevyResponse>(It.IsAny<GetExpiredLevyRequest>()), Times.Never);
        _financeApiClient.Verify(x => x.PostWithResponseCode<List<LevyDeclaration>>(It.IsAny<PostLevyDeclarationSummaryByDate>(), It.IsAny<bool>()), Times.Never);
    }
}
