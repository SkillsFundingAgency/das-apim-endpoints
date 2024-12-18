namespace SFA.DAS.EmployerAan.InnerApi.Settings
{
    public class NotificationLocationsPostApiRequest
    {
        public List<Location> Locations { get; set; } = [];

        public class Location
        {
            public string Name { get; set; }
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
