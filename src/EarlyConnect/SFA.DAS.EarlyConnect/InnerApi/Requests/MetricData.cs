namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class MetricDataList
    {
        public IEnumerable<MetricData> MetricsData { get; set; }
    }
    public class MetricData
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
