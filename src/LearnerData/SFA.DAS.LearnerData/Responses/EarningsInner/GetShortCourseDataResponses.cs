namespace SFA.DAS.LearnerData.Responses.EarningsInner;

public class GetFm99ShortCourseDataResponse
{
    public List<ShortCourseEarning> Earnings { get; set; }
}

public class ShortCourseEarning
{
    public int CollectionYear { get; set; }
    public byte CollectionPeriod { get; set; }
    public Decimal Amount { get; set; }
    public string Type { get; set; }
}
