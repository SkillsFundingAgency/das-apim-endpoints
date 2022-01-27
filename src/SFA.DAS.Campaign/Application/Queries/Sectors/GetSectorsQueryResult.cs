using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Campaign.Application.Queries.Sectors
{
    public class GetSectorsQueryResult
    {
        public IEnumerable<GetRoutesListItem> Sectors { get ; set ; }
    }
}