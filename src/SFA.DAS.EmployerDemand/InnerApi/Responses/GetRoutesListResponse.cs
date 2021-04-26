using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Sectors { get; set; }
    }
}