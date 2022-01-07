using System.Collections.Generic;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetRoutesResponse
    {
        public IEnumerable<GetRouteResponseItem> Routes { get; set; }
    }
    
    public class GetRouteResponseItem
    {
        public string Route { get; set; }
        
        public static implicit operator GetRouteResponseItem (GetRoutesListItem source)
        {
            return new GetRouteResponseItem
            {
                Route = source.Name
            };
        }
    }
}