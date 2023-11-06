namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class MetricDataList
    {
        public IEnumerable<MetricData> ListOfMetricData { get; set; }
    }
    public class MetricData
    {
        public string Region { get; set; }
        public decimal IntendedStartYear { get; set; }
        public int MaxTravelInMiles { get; set; }
        public bool WillingnessToRelocate { get; set; }
        public int NoOfGCSCs { get; set; }
        public int NoOfStudents { get; set; }
        public MetricFlagRequestModel MetricFlagRequestModel { get; set; }
    }
    public class MetricFlagRequestModel
    {
        public IList<string> MetricFlags { get; set; }
    }
}
