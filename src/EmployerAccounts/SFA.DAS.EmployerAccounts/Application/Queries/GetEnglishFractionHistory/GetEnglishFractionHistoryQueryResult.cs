using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory
{
    public class GetEnglishFractionHistoryQueryResult
    {
        public IEnumerable<GetEnglishFractionHistoryResponse.DasEnglishFraction> Fractions { get; set; }
    }
}