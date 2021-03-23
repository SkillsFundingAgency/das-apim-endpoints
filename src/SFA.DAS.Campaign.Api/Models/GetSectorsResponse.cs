using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.Models
{

    public class GetSectorsResponse
    {
        public IEnumerable<GetSectorResponseItem> Sectors { get; set; }
    }
    
    public class GetSectorResponseItem
    {
        public string Route { get; set; }
        
        public static implicit operator GetSectorResponseItem (GetRoutesListItem source)
        {
            return new GetSectorResponseItem
            {
                Route = source.Name
            };
        }
    }
    
}