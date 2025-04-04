namespace SFA.DAS.SharedOuterApi.Models.RoatpV2
{
    public class ProviderAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public static explicit operator ProviderAddress(InnerApi.Responses.RoatpV2.ProviderAddress source)
        {
            if (source == null) return null;

            return new ProviderAddress
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Town = source.Town,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude
            };
        }
    }
}