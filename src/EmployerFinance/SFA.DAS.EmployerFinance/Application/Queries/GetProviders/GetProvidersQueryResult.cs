using System.Collections.Generic;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetProviders
{
    public class GetProvidersQueryResult
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
    }
}