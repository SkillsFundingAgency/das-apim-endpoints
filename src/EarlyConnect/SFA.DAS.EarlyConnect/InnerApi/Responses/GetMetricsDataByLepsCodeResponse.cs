namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetMetricsDataByLepsCodeResponse 
    {
        public ICollection<ApprenticeshipMetricsData>? ListOfMetricsData { get; set; }
    }
    public class ApprenticeshipMetricsData
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public decimal IntendedStartYear { get; set; }
        public int MaxTravelInMiles { get; set; }
        public bool WillingnessToRelocate { get; set; }
        public int NoOfGCSCs { get; set; }
        public int NoOfStudents { get; set; }
        public int LogId { get; set; }
        public DateTime DateAdded { get; set; }
        public IList<MetricsFlag> InterestAreas { get; set; }
    }
    public class MetricsFlag
    {
        public string Flag { get; set; }
        public string FlagCode { get; set; }
        public bool FlagValue { get; set; }
    }
}
