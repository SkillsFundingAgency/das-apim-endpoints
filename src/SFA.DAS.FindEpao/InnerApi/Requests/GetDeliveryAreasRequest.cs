using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetDeliveryAreasRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/ao/delivery-areas";
    }
}