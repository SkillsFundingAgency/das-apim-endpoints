using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup
{
    public class AddressItem
    {
        public string Uprn { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public static implicit operator AddressItem(GetAddressesListItem source) => 
            new AddressItem
            {
                Uprn = source.Uprn,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                Town = source.PostTown,
                County = source.County,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
            };
    }
}
