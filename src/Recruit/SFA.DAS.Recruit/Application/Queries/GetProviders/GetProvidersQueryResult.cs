using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetProviders
{
    public class GetProvidersQueryResult
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
    }
}