using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindEpao.InnerApi.Requests
{
    public class GetDeliveryAreasRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/ao/delivery-areas";
    }
}