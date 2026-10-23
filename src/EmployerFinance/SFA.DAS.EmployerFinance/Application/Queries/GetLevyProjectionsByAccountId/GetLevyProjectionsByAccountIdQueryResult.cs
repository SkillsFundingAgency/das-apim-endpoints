using System;
using System.Collections.Generic;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public sealed record GetLevyProjectionsByAccountIdQueryResult
{
    public long AccountId { get; init; }
    public DateTime LatestLevyDeclarationInDate { get; set; }
    public GetLevySummaryByAccountIdQueryResult Summary { get; set; }
    public List<MonthlyFundingBreakdown> Projections { get; init; } = [];
}