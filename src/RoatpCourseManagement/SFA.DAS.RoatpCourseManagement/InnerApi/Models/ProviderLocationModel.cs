using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models
{
    public class ProviderLocationModel
    {
        public int ProviderLocationId { get; set; }
        public Guid NavigationId { get; set; }
        public string LocationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public bool IsImported { get; set; }
        public LocationType LocationType { get; set; }
    }
}
