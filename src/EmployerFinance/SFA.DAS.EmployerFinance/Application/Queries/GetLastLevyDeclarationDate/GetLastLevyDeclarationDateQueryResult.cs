using System;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLastLevyDeclarationDate;

public sealed record GetLastLevyDeclarationDateQueryResult(DateTime? LatestLevyDeclarationInDate);