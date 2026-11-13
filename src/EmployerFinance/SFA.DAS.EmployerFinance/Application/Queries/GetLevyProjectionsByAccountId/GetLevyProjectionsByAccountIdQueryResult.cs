using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.Models.Projections;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public sealed record GetLevyProjectionsByAccountIdQueryResult
{
    public long AccountId { get; init; }
    public DateTime LatestLevyDeclarationInDate { get; set; }
    public LevyProjectionsSummary Summary { get; set; }
    public List<MonthlyFundingBreakdown> Projections { get; init; } = [];
}