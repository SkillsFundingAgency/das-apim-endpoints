using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class Location
    {
        public string Name { get; set; }
        public LocationPoint LocationPoint { get; set; }
    }
    public class LocationPoint
    {
        public List<double> GeoPoint { get; set; }
    }
}