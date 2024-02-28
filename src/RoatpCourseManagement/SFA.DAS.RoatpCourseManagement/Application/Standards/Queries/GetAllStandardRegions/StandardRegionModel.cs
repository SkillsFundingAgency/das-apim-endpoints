namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions
{
    public class StandardRegionModel
    {
        public int Id { get; set; }
        public string SubregionName { get; set; }
        public string RegionName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsSelected { get; set; }
    }
}
