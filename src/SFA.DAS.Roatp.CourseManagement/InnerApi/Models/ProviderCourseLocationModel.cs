using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models
{
    public class ProviderCourseLocationModel
    {
        public string LocationName { get; set; }
        public LocationType LocationType { get; set; }
        public bool? HasDayReleaseDeliveryOption { get; set; }
        public bool? HasBlockReleaseDeliveryOption { get; set; }
        public string RegionName { get; set; }
        public static implicit operator ProviderCourseLocationModel(GetProviderCourseLocationsResponse source) =>
          new ProviderCourseLocationModel
          {
              LocationName = source.LocationName,
              LocationType = source.LocationType,
              HasDayReleaseDeliveryOption = source.HasDayReleaseDeliveryOption,
              HasBlockReleaseDeliveryOption = source.HasBlockReleaseDeliveryOption,
              RegionName = source.RegionName,
          };
    }
}
