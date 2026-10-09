#nullable enable
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
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public class GetLevyProjectionsByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient)
    : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;
        
        var lastSubmissionTask = financeApiClient.Get<GetLevyLastSubmissionDateResponse>(new GetLevyLastSubmissionDateRequest(request.AccountId));
        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(new GetLevySummaryByAccountIdRequest(request.AccountId));
        
        var historicTransactionLines = await FetchHistoricTransactionLinesDataAsync(now, request.AccountId);
        var levyIn = historicTransactionLines
            .Where(x => x.TransactionType == TransactionItemType.Declaration)
            .GroupBy(x => new DateOnly(x.TransactionDate.Year, x.TransactionDate.Month, 1))
            .Select(x => new LevyInMonthSummary(x.Key, x.Sum(t => t.Amount)))
            .ToList();

        var data = new PostEmployerFundingProjectionRequestData
        {
            Months = request.Months,
            HistoricLevyIn = levyIn
        };
        var projectionTask = projectionApiClient.PostWithResponseCode<EstimatesTimeline>(new PostEmployerFundingProjectionRequest(request.AccountId, data));
        await Task.WhenAll(lastSubmissionTask, levySummaryTask, projectionTask);

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            AccountId = request.AccountId,
            Summary = levySummaryTask.Result,
            Projections = projectionTask.Result.Body.Projections,
            LatestLevyDeclarationInDate = lastSubmissionTask.Result.LastSubmissionDate
        };
    }
    
    private async Task<List<TransactionLine>> FetchHistoricTransactionLinesDataAsync(DateTime now, long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);
        return await financeApiClient.Get<List<TransactionLine>>(new GetAccountTransactionSummaryByDateRequest(accountId, startDate, endDate));
    }
}