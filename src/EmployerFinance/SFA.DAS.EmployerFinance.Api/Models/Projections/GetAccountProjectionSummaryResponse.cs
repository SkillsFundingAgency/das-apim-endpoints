using System;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;

namespace SFA.DAS.EmployerFinance.Api.Models;

public class GetAccountProjectionSummaryResponse
{
    public long AccountId { get; private set; }
    public DateTime ProjectionGenerationDate { get; private set; }
    public int NumberOfMonths { get; private set; }
    public decimal FundsIn { get; private set; }
    public decimal FundsOut { get; private set; }
    public static explicit operator GetAccountProjectionSummaryResponse(GetAccountProjectionSummaryQueryResult source)
    {
        return new GetAccountProjectionSummaryResponse
        {
            AccountId = source.AccountId,
            ProjectionGenerationDate = source.ProjectionGenerationDate.GetValueOrDefault(),
            NumberOfMonths = source.NumberOfMonths,
            FundsIn = source.FundsIn,
            FundsOut = source.FundsOut,
        };
    }
}
