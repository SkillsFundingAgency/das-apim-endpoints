using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class GetClosestRegionRequest : IGetApiRequest
    {
        private readonly double _latitude;
        private readonly double _longitude;

        public GetClosestRegionRequest(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public string GetUrl => $"api/regions/closest?latitude={_latitude}&longitude={_longitude}";
    }
}
