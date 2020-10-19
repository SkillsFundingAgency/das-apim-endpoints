using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetDeliveryAreasRequest : IGetApiRequest
    {
        public string GetUrl => $"api/ao/delivery-areas";
    }
}