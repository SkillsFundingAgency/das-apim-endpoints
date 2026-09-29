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
    public async Task Then_Returns_Empty_Projections_When_No_Transactions(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId, []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projections.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Only_Sums_Declaration_Transactions_For_LevyIn(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var transactionDate = new DateTime(2024, 6, 15);
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = transactionDate, Amount = 500m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = transactionDate, Amount = 200m, TransactionType = TransactionItemType.Payment },
            new TransactionLine { TransactionDate = transactionDate, Amount = 100m, TransactionType = TransactionItemType.Transfer }
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections.Should().HaveCount(1);
        result.Projections[0].LevyIn.Should().Be(500m);
    }

    [Test, MoqAutoData]
    public async Task Then_Groups_And_Sums_Declaration_Transactions_By_Month(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = new DateTime(2024, 1, 10), Amount = 500m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 1, 20), Amount = 300m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 1, 25), Amount = 999m, TransactionType = TransactionItemType.Payment },
            new TransactionLine { TransactionDate = new DateTime(2024, 2, 5),  Amount = 400m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 2, 25), Amount = 100m, TransactionType = TransactionItemType.Declaration }
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections.Should().HaveCount(2);
        result.Projections.Should().ContainSingle(p =>
            p.CalendarPeriodMonth == 1 && p.CalendarPeriodYear == 2024 && p.LevyIn == 800m);
        result.Projections.Should().ContainSingle(p =>
            p.CalendarPeriodMonth == 2 && p.CalendarPeriodYear == 2024 && p.LevyIn == 500m);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Projections_In_Descending_Chronological_Order(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = new DateTime(2024, 1, 1), Amount = 100m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 3, 1), Amount = 300m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 2, 1), Amount = 200m, TransactionType = TransactionItemType.Declaration }
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections.Should().HaveCount(3);
        result.Projections[0].CalendarPeriodMonth.Should().Be(3);
        result.Projections[1].CalendarPeriodMonth.Should().Be(2);
        result.Projections[2].CalendarPeriodMonth.Should().Be(1);
    }

    [Test, MoqAutoData]
    public async Task Then_Orders_Descending_Correctly_Across_Different_Years(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = new DateTime(2023, 12, 1), Amount = 300m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2024, 12, 1), Amount = 500m, TransactionType = TransactionItemType.Declaration },
            new TransactionLine { TransactionDate = new DateTime(2023, 12, 15), Amount = 200m, TransactionType = TransactionItemType.Declaration }
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections.Should().HaveCount(2);
        result.Projections[0].CalendarPeriodYear.Should().Be(2024);
        result.Projections[0].LevyIn.Should().Be(500m);
        result.Projections[1].CalendarPeriodYear.Should().Be(2023);
        result.Projections[1].LevyIn.Should().Be(500m);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Zero_LevyIn_When_No_Declaration_Transactions_In_Month(
        GetLevyProjectionsByAccountIdQuery query,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId,
        [
            new TransactionLine { TransactionDate = new DateTime(2024, 6, 1), Amount = 500m, TransactionType = TransactionItemType.Payment },
            new TransactionLine { TransactionDate = new DateTime(2024, 6, 15), Amount = 300m, TransactionType = TransactionItemType.Transfer }
        ]);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Projections.Should().HaveCount(1);
        result.Projections[0].CalendarPeriodMonth.Should().Be(6);
        result.Projections[0].CalendarPeriodYear.Should().Be(2024);
        result.Projections[0].LevyIn.Should().Be(0m);
    }

    [Test, MoqAutoData]
    public async Task Then_Uses_Months_From_Request_To_Define_Date_Range(
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var query = new GetLevyProjectionsByAccountIdQuery(123, 12);

        SetupTransactionsResponse(mockFinanceApiClient, query.AccountId, []);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        mockFinanceApiClient.Verify(c => c.Get<List<TransactionLine>>(
            It.Is<GetAccountTransactionSummaryByDateRequest>(r =>
                r.AccountId == query.AccountId)), Times.Once);
    }
}