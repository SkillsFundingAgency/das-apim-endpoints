using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

public class GetClosestRegionRequest(double latitude, double longitude) : IGetApiRequest
{
    public string GetUrl => $"api/regions/closest?latitude={latitude}&longitude={longitude}";
}