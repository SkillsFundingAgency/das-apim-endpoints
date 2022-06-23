using System;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models
{
    public class RegionModel
    {
        public int Id { get; set; }
        public string SubregionName { get; set; }
        public string RegionName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
