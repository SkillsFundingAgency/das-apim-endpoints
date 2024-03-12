using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class DeliveryUpdateMapper
    {
        public static DeliveryUpdate MapFromDeliveryUpdateRequest(this DeliveryUpdatePostRequest request)
        {
            var deliveryUpdate = new DeliveryUpdate
            {
                Source = request.Source,
                Ids = request.Ids,
            };

            return deliveryUpdate;
        }
    }
}
