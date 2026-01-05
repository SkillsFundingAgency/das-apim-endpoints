using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetMetricsDataByLepsCodeResponse
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
        public IList<MetricsFlagResponse> InterestAreas { get; set; }
        public static implicit operator GetMetricsDataByLepsCodeResponse(ApprenticeshipMetricsData source)
        {
            return new GetMetricsDataByLepsCodeResponse
            {
                Id = source.Id,
                Region = source.Region,
                IntendedStartYear = source.IntendedStartYear,
                MaxTravelInMiles = source.MaxTravelInMiles,
                WillingnessToRelocate = source.WillingnessToRelocate,
                NoOfGCSCs = source.NoOfGCSCs,
                NoOfStudents = source.NoOfStudents,
                LogId = source.LogId,
                DateAdded = source.DateAdded,
                InterestAreas = source.InterestAreas.Select(c => (MetricsFlagResponse)c).ToList()
            };
        }
    }

    public class MetricsFlagResponse
    {
        public string Flag { get; set; }
        public string FlagCode { get; set; }
        public bool FlagValue { get; set; }
        public static implicit operator MetricsFlagResponse(MetricsFlag source)
        {
            return new MetricsFlagResponse
            {
                Flag = source.Flag,
                FlagCode = source.FlagCode,
                FlagValue = source.FlagValue
            };
        }
    }
}