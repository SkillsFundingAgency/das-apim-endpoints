using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public sealed record GetEmployerFundingProjectionByAccountIdResponse
{
    public List<MonthlyFundingBreakdown> FundingBreakdowns { get; init; } = [];
}