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

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public class GetLevyProjectionsByAccountIdQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient) : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var toDate = DateTime.UtcNow;
        var startDate = DateTime.UtcNow.AddMonths(-request.Months);

        var response = await financeApiClient.Get<List<TransactionLine>>(
            new GetAccountTransactionSummaryByDateRequest(request.AccountId, startDate, toDate));

        var projections = response
            .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
            .Select(g => new MonthlyBreakdown
            {
                CalendarPeriodMonth = g.Key.Month,
                CalendarPeriodYear = g.Key.Year,
                CalendarMonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                LevyIn = g.Where(t => t.TransactionType == TransactionItemType.Declaration).Sum(t => t.Amount)
            })
            .OrderBy(x => x.CalendarPeriodYear)
            .ThenBy(x => x.CalendarPeriodMonth)
            .ToList();

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            Projections = projections
        };
    }
}