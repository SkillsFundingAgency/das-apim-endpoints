using System;
using SFA.DAS.EmployerFinance.Models.Projections;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public sealed record GetLevyProjectionsByAccountIdQueryResult
{
    public DateTime LatestLevyDeclarationInDate { get; set; }
    public IReadOnlyList<MonthlyBreakdown> Projections { get; init; } = [];
}