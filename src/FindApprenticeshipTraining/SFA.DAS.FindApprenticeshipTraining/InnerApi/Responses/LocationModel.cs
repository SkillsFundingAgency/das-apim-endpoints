namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class LocationModel
{
    public long Ordering { get; set; }
    public bool AtEmployer { get; set; }
    public bool BlockRelease { get; set; }
    public bool DayRelease { get; set; }
    public int LocationType { get; set; }
    public string CourseLocation { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
    public double CourseDistance { get; set; }
}
