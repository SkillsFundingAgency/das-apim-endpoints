using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Responses
{
    public class GetProviderCourseLocationsResponse
    {
        public int Id { get; set; }
        public Guid NavigationId { get; set; }
        public int ProviderCourseId { get; set; }
        public int? ProviderLocationId { get; set; }
        public decimal Radius { get; set; }
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
        public bool? OffersPortableFlexiJob { get; set; }
        public bool IsImported { get; set; }
        public string LocationName { get; set; }
        public LocationType LocationType { get; set; }
    }
}
