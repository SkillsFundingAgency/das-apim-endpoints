using System.Collections.Generic;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetDeliveryAreaListResponse
    {
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
    }
}