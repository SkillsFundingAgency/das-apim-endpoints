using System.Collections.Generic;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Routes { get; set; }
    }
    
    public class GetRoutesListItem
    {
        public string Name { get; set; }
    }
}