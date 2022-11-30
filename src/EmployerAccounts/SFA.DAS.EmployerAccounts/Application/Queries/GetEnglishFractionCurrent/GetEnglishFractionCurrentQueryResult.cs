using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent
{
    public class GetEnglishFractionCurrentQueryResult
    {
        public IEnumerable<GetEnglishFractionCurrentResponse.DasEnglishFraction> Fractions { get; set; }
    }
}