#nullable enable
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
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

public class GetLevyProjectionsByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient)
    : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;

        var lastSubmissionTask = financeApiClient.Get<GetLevyLastSubmissionDateResponse>(
            new GetLevyLastSubmissionDateRequest(request.AccountId));

        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(
            new GetLevySummaryByAccountIdRequest(request.AccountId));

        var declarations = await FetchHistoricTransactionLinesDataAsync(now, request.AccountId);

        var projectionTask = projectionApiClient.PostWithResponseCode<EstimatesTimeline>(
            new PostEmployerFundingProjectionRequest(request.AccountId, new PostEmployerFundingProjectionRequestData
            {
                Months = request.Months,
                HistoricLevyIn = BuildLevyInSummary(declarations)
            }));

        await Task.WhenAll(lastSubmissionTask, levySummaryTask, projectionTask);

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            AccountId = request.AccountId,
            Summary = levySummaryTask.Result,
            Projections = projectionTask.Result.Body.Projections,
            LatestLevyDeclarationInDate = lastSubmissionTask.Result.LastSubmissionDate ?? DateTime.MinValue
        };
    }

    private static List<LevyInMonthSummary> BuildLevyInSummary(List<LevyDeclaration> declarations) =>
    [
        .. declarations
            .Where(t => t.PayrollDate() != null)
            .GroupBy(t => t.PayrollDate()!.Value)
            .Select(g => new LevyInMonthSummary(
                new DateOnly(g.Key.Year, g.Key.Month, 1),
                g.Sum(t => t.TotalAmount - t.TopUp)))
    ];

    private async Task<List<LevyDeclaration>> FetchHistoricTransactionLinesDataAsync(DateTime now, long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);
        return await financeApiClient.Get<List<LevyDeclaration>>(new GetLevyDeclarationSummaryByDate(accountId, startDate, endDate));
    }
}