using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get; set; }
        
    }
}