using System.Collections.Generic;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetProviders
{
    public class GetProvidersQueryResult
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
    }
}