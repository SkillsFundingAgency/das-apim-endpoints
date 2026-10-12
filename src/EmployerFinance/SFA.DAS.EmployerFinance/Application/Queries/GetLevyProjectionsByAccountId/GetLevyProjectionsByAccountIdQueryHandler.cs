#nullable enable
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public class GetLevyProjectionsByAccountIdQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;

        var historicDataTask = FetchHistoricDataAsync(now, request.AccountId);
        var lastSubmissionTask = financeApiClient.Get<GetLevyLastSubmissionDateResponse>(
            new GetLevyLastSubmissionDateRequest(request.AccountId));
        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(
            new GetLevySummaryByAccountIdRequest(request.AccountId));

        await Task.WhenAll(historicDataTask, lastSubmissionTask, levySummaryTask);

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            Projections = ProjectFromHistoricData(now, request.Months, historicDataTask.Result, levySummaryTask.Result),
            LatestLevyDeclarationInDate = lastSubmissionTask.Result.LastSubmissionDate ?? DateTime.MinValue
        };
    }

    private async Task<Dictionary<(int Year, int Month), MonthlyBreakdown>> FetchHistoricDataAsync(
        DateTime now,
        long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);

        var response = await financeApiClient.Get<List<LevyDeclaration>>(new GetLevyDeclarationSummaryByDate(accountId, startDate, endDate));

        return response
            .Where(t => t.PayrollDate() != null)
            .GroupBy(t => t.PayrollDate())
            .ToDictionary(
                g => (g.Key.Value.Year, g.Key.Value.Month),
                g => new MonthlyBreakdown
                {
                    CalendarPeriodYear = g.Key.Value.Year,
                    CalendarPeriodMonth = g.Key.Value.Month,
                    CalendarMonthName = g.Key.Value.ToString("MMMM"),
                    LevyIn = g.Sum(t => t.TotalAmount - t.TopUp),
                    ExpiredLevy = 0m,             // will be populated in future stories.
                    CommittedLearnerCosts = 0,    // will be populated in future stories.
                    CommittedTransferCosts = 0    // will be populated in future stories.
                });
    }

    /// <summary>
    /// Projects future levy data based on historical information.
    /// </summary>
    private static IReadOnlyList<MonthlyBreakdown> ProjectFromHistoricData(
        DateTime now,
        int months,
        Dictionary<(int Year, int Month), MonthlyBreakdown> historic,
        GetLevySummaryByAccountIdResponse levySummary)
    {
        var projections = new List<MonthlyBreakdown>(months);
        var runningClosingLevyBalance = levySummary.CurrentLevyFunds;

        for (var i = 0; i < months; i++)
        {
            var pointInTime = now.AddMonths(i);
            var levyData = ResolveLevyData(historic, pointInTime, isCurrentMonth: i == 0);

            var levyIn = levyData?.LevyIn ?? 0m;
            var levyOut = levyData?.LevyOut ?? 0m;

            // Current month uses the actual live balance as-is.
            // Forecast months roll forward by applying levy in/out to the previous closing balance.
            runningClosingLevyBalance = CalculateClosingBalance(runningClosingLevyBalance, levyIn, levyOut, isCurrentMonth: i == 0);

            projections.Add(new MonthlyBreakdown
            {
                CalendarPeriodYear = pointInTime.Year,
                CalendarPeriodMonth = pointInTime.Month,
                CalendarMonthName = pointInTime.ToString("MMMM"),
                LevyIn = levyIn,
                ExpiredLevy = levyData?.ExpiredLevy ?? 0m,
                ClosingLevyBalance = runningClosingLevyBalance,
                CommittedLearnerCosts = levyData?.CommittedLearnerCosts ?? 0m, // will be populated in future stories.
                CommittedTransferCosts = levyData?.CommittedTransferCosts ?? 0m // will be populated in future stories.
            });
        }

        return projections;
    }

    /// <summary>
    /// Current month adds levy in to the live balance (levy out already reflected in CurrentLevyFunds).
    /// Forecast months apply levy in/out to the previous closing balance, floored at zero.
    /// </summary>
    private static decimal CalculateClosingBalance(decimal currentBalance, decimal levyIn, decimal levyOut, bool isCurrentMonth)
    {
        return isCurrentMonth
            ? currentBalance + levyIn
            : currentBalance + levyIn - levyOut;
    }

    /// <summary>
    /// Current month uses actual data if already declared, otherwise falls back
    /// to the same month last year. Forecast months always use the same month last year.
    /// </summary>
    private static MonthlyBreakdown? ResolveLevyData(Dictionary<(int Year, int Month), MonthlyBreakdown> historic,
        DateTime pointInTime,
        bool isCurrentMonth)
    {
        if (isCurrentMonth && historic.TryGetValue((pointInTime.Year, pointInTime.Month), out var current))
        {
            return current;
        }

        return historic.GetValueOrDefault((pointInTime.Year - 1, pointInTime.Month));
    }
}