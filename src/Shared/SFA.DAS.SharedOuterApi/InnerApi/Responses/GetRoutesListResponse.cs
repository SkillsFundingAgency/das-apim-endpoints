using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Routes { get; set; }   
    }
    
    public class GetRoutesListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}