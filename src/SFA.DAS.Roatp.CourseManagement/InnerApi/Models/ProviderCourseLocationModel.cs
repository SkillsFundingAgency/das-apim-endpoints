namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models
{
    public class ProviderCourseLocationModel
    {
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
        public bool? OffersPortableFlexiJob { get; set; }
        public string LocationName { get; set; }
        public LocationType LocationType { get; set; }
    }
}
