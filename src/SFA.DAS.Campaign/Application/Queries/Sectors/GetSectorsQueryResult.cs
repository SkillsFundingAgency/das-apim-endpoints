using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Application.Queries.Sectors
{
    public class GetSectorsQueryResult
    {
        public IEnumerable<GetSectorsListItem> Sectors { get ; set ; }
    }
}