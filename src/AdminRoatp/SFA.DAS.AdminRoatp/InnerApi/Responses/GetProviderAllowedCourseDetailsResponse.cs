namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetProviderAllowedCourseDetailsResponse
{
    public DateTime? LastDateStarts { get; set; }
    public bool IsCourseRestricted { get; set; }
    public bool IsClosedToNewStarts { get; set; }
    public bool IsActive { get; set; }
}
