using System.Collections.Generic;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetRoutesResponse
    {
        public IEnumerable<GetRouteResponseItem> Routes { get; set; }
    }
    
    public class GetRouteResponseItem
    {
        public string Name { get; set; }
        
        public static implicit operator GetRouteResponseItem (GetRoutesListItem source)
        {
            return new GetRouteResponseItem
            {
                Name = source.Name
            };
        }
    }
}