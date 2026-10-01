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

    private async Task<List<MonthlyBreakdown>> FetchHistoricDataAsync(DateTime now, long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        
        // this gives use the last 12 months + any potential current month levy in
        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddSeconds(-1);

        var response = await financeApiClient.Get<List<TransactionLine>>(
            new GetAccountTransactionSummaryByDateRequest(accountId, startDate, endDate));
        
        return
        [
            .. response
                .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
                .Select(g => new MonthlyBreakdown
                {
                    CalendarPeriodMonth = g.Key.Month,
                    CalendarPeriodYear = g.Key.Year,
                    CalendarMonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    LevyIn = g.Where(t => t.TransactionType == TransactionItemType.Declaration).Sum(t => t.Amount),
                    ExpiredLevy = g.Where(t => t.TransactionType is TransactionItemType.ExpiredFund or TransactionItemType.ShortExpiredFund).Sum(t => t.Amount)
                })
                .OrderBy(x => x.CalendarPeriodYear)
                .ThenBy(x => x.CalendarPeriodMonth)
        ];
    }

    private static IReadOnlyList<MonthlyBreakdown> ProjectFromHistoricData(DateTime now, int months, List<MonthlyBreakdown> historicLevyIn)
    {
        var pointInTime = now;
        var projections = new List<MonthlyBreakdown>();
        for (var i = 0; i < months; i++)
        {
            MonthlyBreakdown levyData = null;
            if (pointInTime.Month == now.Month && pointInTime.Year == now.Year)
            {
                levyData = historicLevyIn.FirstOrDefault(x => x.CalendarPeriodMonth == pointInTime.Month && x.CalendarPeriodYear == pointInTime.Year);
            }

            levyData ??= historicLevyIn.FirstOrDefault(x => x.CalendarPeriodMonth == pointInTime.Month && x.CalendarPeriodYear == pointInTime.Year - 1);
            projections.Add(new MonthlyBreakdown
            {
                CalendarPeriodMonth = pointInTime.Month,
                CalendarPeriodYear = pointInTime.Year,
                CalendarMonthName = pointInTime.ToString("MMMM"),
                LevyIn = levyData?.LevyIn ?? 0m,
                ExpiredLevy = levyData?.ExpiredLevy ?? 0m
            });

            pointInTime = pointInTime.AddMonths(1);
        }

        return projections;
    }
}