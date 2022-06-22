using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Application.Epaos.Queries.GetDeliveryAreaList
{
    public class GetDeliveryAreaListResult
    {
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
    }
}