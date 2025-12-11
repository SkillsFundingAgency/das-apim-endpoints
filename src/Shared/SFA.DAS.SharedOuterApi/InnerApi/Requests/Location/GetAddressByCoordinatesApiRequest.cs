using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
public record GetAddressByCoordinatesApiRequest(double Latitude, double Longitude) : IGetApiRequest
{
    public string GetUrl => $"api/addresses/ByCoordinates?latitude={Latitude}&longitude={Longitude}";
}