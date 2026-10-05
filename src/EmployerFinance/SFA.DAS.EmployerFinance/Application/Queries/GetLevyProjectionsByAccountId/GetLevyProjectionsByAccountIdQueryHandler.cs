using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.Models.Enums;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

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

        await Task.WhenAll(historicDataTask, lastSubmissionTask);

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            Projections = ProjectFromHistoricData(now, request.Months, historicDataTask.Result),
            LatestLevyDeclarationInDate = lastSubmissionTask.Result.LastSubmissionDate
        };
    }

    private async Task<Dictionary<(int Year, int Month), MonthlyBreakdown>> FetchHistoricDataAsync(DateTime now,
        long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);

        var response = await financeApiClient.Get<List<TransactionLine>>(
            new GetAccountTransactionSummaryByDateRequest(accountId, startDate, endDate));

        return response
            .GroupBy(t => (t.TransactionDate.Year, t.TransactionDate.Month))
            .ToDictionary(
                g => g.Key,
                g => new MonthlyBreakdown
                {
                    CalendarPeriodYear = g.Key.Year,
                    CalendarPeriodMonth = g.Key.Month,
                    CalendarMonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    LevyIn = g
                        .Where(t => t.TransactionType == TransactionItemType.Declaration)
                        .Sum(t => t.Amount),
                    ExpiredLevy = g
                        .Where(t => t.TransactionType is TransactionItemType.ExpiredFund
                            or TransactionItemType.ShortExpiredFund)
                        .Sum(t => t.Amount),
                    CommittedLearnerCosts = 0,
                    CommittedTransferCosts = 0 // will be populated in future stories.
                });
    }


    private static IReadOnlyList<MonthlyBreakdown> ProjectFromHistoricData(DateTime now,
        int months,
        Dictionary<(int Year, int Month), MonthlyBreakdown> historic)
    {
        var projections = new List<MonthlyBreakdown>(months);

        // Seed: use current month's actual data if already declared,
        // otherwise fall back to the same month last year.
        var currentMonthData = historic.GetValueOrDefault((now.Year, now.Month))
                            ?? historic.GetValueOrDefault((now.Year - 1, now.Month));
        var runningClosingLevy = (currentMonthData?.LevyIn ?? 0m)
                               - (currentMonthData?.LevyOut ?? 0m);

        for (var i = 0; i < months; i++)
        {
            var pointInTime = now.AddMonths(i);

            if (i == 0)
            {
                projections.Add(new MonthlyBreakdown
                {
                    CalendarPeriodYear = pointInTime.Year,
                    CalendarPeriodMonth = pointInTime.Month,
                    CalendarMonthName = pointInTime.ToString("MMMM"),
                    LevyIn = currentMonthData?.LevyIn ?? 0m,
                    ExpiredLevy = currentMonthData?.ExpiredLevy ?? 0m,
                    ClosingLevy = runningClosingLevy,
                });

                continue;
            }

            // Forecast months: use same month from last year as the projection basis.
            var levyData = historic.GetValueOrDefault((pointInTime.Year - 1, pointInTime.Month));

            var levyIn = levyData?.LevyIn ?? 0m;
            var levyOut = levyData?.LevyOut ?? 0m; // ExpiredLevy + CommittedLearnerCosts + CommittedTransferCosts

            runningClosingLevy = Math.Max(0m, runningClosingLevy + levyIn - levyOut);

            projections.Add(new MonthlyBreakdown
            {
                CalendarPeriodYear = pointInTime.Year,
                CalendarPeriodMonth = pointInTime.Month,
                CalendarMonthName = pointInTime.ToString("MMMM"),
                LevyIn = levyIn,
                ExpiredLevy = levyData?.ExpiredLevy ?? 0m,
                ClosingLevy = runningClosingLevy,
            });
        }

        return projections;
    }
}