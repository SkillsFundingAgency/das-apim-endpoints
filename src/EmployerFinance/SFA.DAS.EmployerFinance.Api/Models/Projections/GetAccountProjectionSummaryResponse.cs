using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.Models
{
    public class GetAccountProjectionSummaryResponse
    {
        public long AccountId { get; private set; }
        public DateTime ProjectionGenerationDate { get; private set; }
        public int NumberOfMonths { get; private set; }
        public decimal FundsIn { get; private set; }
        public decimal FundsOut { get; private set; }
        public IEnumerable<ExpiryAmount> ExpiryAmounts { get; private set; }

        public static explicit operator GetAccountProjectionSummaryResponse(GetAccountProjectionSummaryQueryResult source)
        {
            return new GetAccountProjectionSummaryResponse
            {
                AccountId = source.AccountId,
                ProjectionGenerationDate = source.ProjectionGenerationDate.GetValueOrDefault(),
                NumberOfMonths = source.NumberOfMonths,
                FundsIn = source.FundsIn,
                FundsOut = source.FundsOut,
                ExpiryAmounts = source.ExpiryAmounts.Select(exp => (ExpiryAmount)exp)
            };
        }

    }

    public class ExpiryAmount
    {
        public decimal Amount { get; set; }
        public DateTime PayrollDate { get; set; }

        public static explicit operator ExpiryAmount(GetExpiringFundsListItem source)
        {
            return new ExpiryAmount
            {
                Amount = source.Amount,
                PayrollDate = source.PayrollDate
            };
        }
    }
}
