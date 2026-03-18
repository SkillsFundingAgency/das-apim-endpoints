using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
public record GetAddressByCoordinatesApiRequest(double Latitude, double Longitude) : IGetApiRequest
{
    public string GetUrl => $"api/addresses/ByCoordinates?latitude={Latitude}&longitude={Longitude}";
}