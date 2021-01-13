using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProvidersResult
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
    }
}