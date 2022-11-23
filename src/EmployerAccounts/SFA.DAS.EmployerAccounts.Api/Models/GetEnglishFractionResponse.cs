using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetEnglishFractionResponse
    {
        public IEnumerable<DasEnglishFraction> Fractions { get; set; }

        public class DasEnglishFraction
        {
            public string Id { get; set; }
            public DateTime DateCalculated { get; set; }
            public decimal Amount { get; set; }
            public string EmpRef { get; set; }

            public static implicit operator DasEnglishFraction(InnerApi.Responses.GetEnglishFractionResponse.DasEnglishFraction dasEnglishFraction)
            {
                return new DasEnglishFraction()
                {
                    Id = dasEnglishFraction.Id,
                    DateCalculated = dasEnglishFraction.DateCalculated,
                    Amount = dasEnglishFraction.Amount,
                    EmpRef = dasEnglishFraction.EmpRef
                };
            }
        }

        public static implicit operator GetEnglishFractionResponse(GetEnglishFractionHistoryQueryResult source)
        {
            return new GetEnglishFractionResponse
            {
                Fractions = source.Fractions?.Select(x => (DasEnglishFraction)x)
            };
        }

        public static implicit operator GetEnglishFractionResponse(GetEnglishFractionCurrentQueryResult source)
        {
            return new GetEnglishFractionResponse
            {
                Fractions = source.Fractions?
                    .Where(x => x != null)
                    .Select(x => (DasEnglishFraction)x)
            };
        }
    }
}


