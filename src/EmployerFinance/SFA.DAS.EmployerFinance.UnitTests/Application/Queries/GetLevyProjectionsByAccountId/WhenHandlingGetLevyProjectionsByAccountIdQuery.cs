using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
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
        List<LevyDeclaration> response)
    {
        mock.Setup(c => c.Get<List<LevyDeclaration>>(
                It.Is<GetLevyDeclarationSummaryByDate>(r =>
                    r.AccountId == accountId 
                    && r.FromDate == It.IsAny<DateTime>()
                    && r.ToDate == It.IsAny<DateTime>())))
            .ReturnsAsync(response);
    }

    private static void SetupLastSubmissionDateResponse(
        Mock<IFinanceApiClient<FinanceApiConfiguration>> mock,
        long accountId,
        GetLevyLastSubmissionDateResponse response)
    {
        mock.Setup(c => c.Get<GetLevyLastSubmissionDateResponse>(
                It.Is<GetLevyLastSubmissionDateRequest>(r =>
                    r.AccountId == accountId)))
            .ReturnsAsync(response);
    }

    private static void SetupGetLevySummary(Mock<IFinanceApiClient<FinanceApiConfiguration>> mock,
        long accountId,
        GetLevySummaryByAccountIdResponse response)
    {
        mock.Setup(c => c.Get<GetLevySummaryByAccountIdResponse>(
                It.Is<GetLevySummaryByAccountIdRequest>(r =>
                    r.AccountId == accountId)))
            .ReturnsAsync(response);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Default_Number_Of_Months_Of_Projections_Are_Returned_With_Empty_Values_When_No_Previous_Transactions_Are_Available(
        long accountId,
        DateTime lastSubmissionDate,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);
        SetupLastSubmissionDateResponse(mockFinanceApiClient, accountId, new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });
        SetupGetLevySummary(mockFinanceApiClient, accountId, new GetLevySummaryByAccountIdResponse { CurrentLevyFunds = 0m });


        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projections.Should().HaveCount(query.Months);
        result.Projections.Should().OnlyContain(p => p.LevyIn == 0);
        result.Projections[0].CalendarPeriodMonth.Should().Be(now.Month);
    }
    
    
    
    
    [Test, MoqAutoData]
    public async Task Then_The_Projected_Months_Are_In_Chronological_Order(
        long accountId,
        DateTime lastSubmissionDate,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var now = DateTime.UtcNow;
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);
        SetupLastSubmissionDateResponse(mockFinanceApiClient, accountId, new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });
        SetupGetLevySummary(mockFinanceApiClient, accountId, new GetLevySummaryByAccountIdResponse { CurrentLevyFunds = 0m });

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
        DateTime lastSubmissionDate,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var query = new GetLevyProjectionsByAccountIdQuery(accountId, 17);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);
        SetupLastSubmissionDateResponse(mockFinanceApiClient, accountId, new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });
        SetupGetLevySummary(mockFinanceApiClient, accountId, new GetLevySummaryByAccountIdResponse { CurrentLevyFunds = 0m });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Projections.Should().HaveCount(query.Months);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Latest_Levy_Declaration_Date_Is_Returned(
        long accountId,
        DateTime lastSubmissionDate,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockFinanceApiClient,
        [Greedy] GetLevyProjectionsByAccountIdQueryHandler handler)
    {
        // Arrange
        var query = new GetLevyProjectionsByAccountIdQuery(accountId);
        SetupTransactionsResponse(mockFinanceApiClient, accountId, []);
        SetupLastSubmissionDateResponse(mockFinanceApiClient, accountId, new GetLevyLastSubmissionDateResponse { LastSubmissionDate = lastSubmissionDate });
        SetupGetLevySummary(mockFinanceApiClient, accountId, new GetLevySummaryByAccountIdResponse { CurrentLevyFunds = 0m });
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.LatestLevyDeclarationInDate.Should().Be(lastSubmissionDate);
    }
}