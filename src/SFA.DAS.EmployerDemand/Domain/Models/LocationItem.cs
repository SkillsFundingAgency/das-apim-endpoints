namespace SFA.DAS.EmployerDemand.Domain.Models
{
    public class LocationItem
    {
        public string Name { get ;}
        public double[] GeoPoint { get ;}

        public LocationItem (string name, double[] locationGeoPoint)
        {
            Name  = name;
            GeoPoint = locationGeoPoint;
        }
    }
}