using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;

namespace SFA.DAS.Campaign.Application.Queries.Sectors
{
    public class GetSectorsQueryResult
    {
        public IEnumerable<GetRoutesListItem> Sectors { get ; set ; }
    }
}