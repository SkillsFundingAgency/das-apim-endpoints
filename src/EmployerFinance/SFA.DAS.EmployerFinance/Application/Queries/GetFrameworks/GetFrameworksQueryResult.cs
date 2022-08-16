using System.Collections.Generic;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetFrameworks
{
    public class GetFrameworksQueryResult
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get ; set ; }
    }
}