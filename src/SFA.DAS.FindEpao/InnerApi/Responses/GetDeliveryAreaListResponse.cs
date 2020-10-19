using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetDeliveryAreaListResponse
    {
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
    }
}