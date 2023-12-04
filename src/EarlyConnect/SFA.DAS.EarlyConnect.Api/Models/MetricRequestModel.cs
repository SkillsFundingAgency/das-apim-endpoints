
namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class MetricRequestModel
    {
        public string Region { get; set; }
        public decimal IntendedStartYear { get; set; }
        public int MaxTravelInMiles { get; set; }
        public bool WillingnessToRelocate { get; set; }
        public int NoOfGCSCs { get; set; }
        public int NoOfStudents { get; set; }
        public int LogId { get; set; }
        public IList<string> MetricFlags { get; set; }
    }
}
