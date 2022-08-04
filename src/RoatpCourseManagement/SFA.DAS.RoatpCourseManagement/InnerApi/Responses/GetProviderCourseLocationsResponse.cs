using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses
{
    public class GetProviderCourseLocationsResponse
    {
        public Guid NavigationId { get; set; }
        public int ProviderCourseId { get; set; }
        public int? ProviderLocationId { get; set; }
        public decimal Radius { get; set; }
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
        public bool IsImported { get; set; }
        public string LocationName { get; set; }
        public LocationType LocationType { get; set; }
        public string RegionName { get; set; }
        public int? RegionId { get; set; }
    }
}
