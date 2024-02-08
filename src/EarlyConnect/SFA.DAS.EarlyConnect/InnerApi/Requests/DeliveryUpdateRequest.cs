using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class DeliveryUpdateRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public DeliveryUpdateRequest(DeliveryUpdate deliveryUpdate)
        {
            Data = deliveryUpdate;
        }

        public string PostUrl => "api/delivery-update";
    }
}