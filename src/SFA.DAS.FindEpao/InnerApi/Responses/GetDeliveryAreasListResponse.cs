using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetDeliveryAreasListResponse
    {
        public IEnumerable<GetDeliveryAreasListItem> DeliveryAreas { get; set; }
    }
}