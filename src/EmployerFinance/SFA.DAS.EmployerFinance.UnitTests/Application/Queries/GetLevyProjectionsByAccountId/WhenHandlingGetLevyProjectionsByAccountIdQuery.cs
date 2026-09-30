using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.Models.Enums;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetLevyProjectionsByAccountId;

[TestFixture]
internal class WhenHandlingGetLevyProjectionsByAccountIdQuery
{
    private static void SetupTransactionsResponse(
        Mock<IFinanceApiClient<FinanceApiConfiguration>> mock,
        long accountId,
        List<TransactionLine> response)
    {
        mock.Setup(c => c.Get<List<TransactionLine>>(
                It.Is<GetAccountTransactionSummaryByDateRequest>(r =>
                    r.AccountId == accountId)))
            .ReturnsAsync(response);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Default_Number_Of_Months_Of_Projections_Are_Returned_With_Empty_Values_When_No_Previous_Transactions_Are_Available(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projections.Should().HaveCount(query.Months);
        result.Projections.Should().OnlyContain(p => p.LevyIn == 0);
        result.Projections[0].CalendarPeriodMonth.Should().Be(now.Month);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Current_Month_Levy_Will_Be_Used_Instead_Of_The_Previous_Year_Amount_If_Available(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = now.AddYears(-1), Amount = 200m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now, Amount = 500m, TransactionType = TransactionItemType.Declaration },
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections[0].LevyIn.Should().Be(500m);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Forecasted_Months_Will_Use_The_12_Month_Prior_Figure(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = now.AddMonths(-12), Amount = 12m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-11), Amount = 11m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-10), Amount = 10m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-9), Amount = 9m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-8), Amount = 8m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-7), Amount = 7m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-6), Amount = 6m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-5), Amount = 5m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-4), Amount = 4m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-3), Amount = 3m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-2), Amount = 2m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now.AddMonths(-1), Amount = 1m, TransactionType = TransactionItemType.Declaration },
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections[0].LevyIn.Should().Be(12m);
        result.Projections[1].LevyIn.Should().Be(11m);
        result.Projections[2].LevyIn.Should().Be(10m);
        result.Projections[3].LevyIn.Should().Be(9m);
        result.Projections[4].LevyIn.Should().Be(8m);
        result.Projections[5].LevyIn.Should().Be(7m);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Only_Sums_Declaration_Transactions_For_LevyIn(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Payment },
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Transfer },
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections[0].LevyIn.Should().Be(1m);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Multiple_Declarations_For_The_Same_Month_Are_Totalled(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = now, Amount = 1m, TransactionType = TransactionItemType.Declaration },
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections[0].LevyIn.Should().Be(3m);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Projected_Months_Are_In_Chronological_Order(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        foreach (var projection in result.Projections)
        {
            projection.CalendarPeriodMonth.Should().Be(now.Month);
            projection.CalendarPeriodYear.Should().Be(now.Year);
            now = now.AddMonths(1);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Number_Of_Requested_Months_Is_Returned(
        long accountId,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var query = new GetLevyProjectionsByAccountIdQuery(accountId, 17);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projections.Should().HaveCount(query.Months);
    }
}