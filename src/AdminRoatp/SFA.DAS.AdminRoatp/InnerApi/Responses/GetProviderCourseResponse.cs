namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetProviderCourseResponse
{
    public int ProviderCourseId { get; set; }
    public string StandardUId { get; set; } = string.Empty;
    public string IfateReferenceNumber { get; set; } = string.Empty;
    public string LarsCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Level { get; set; }
    public string ApprovalBody { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string StandardInfoUrl { get; set; } = string.Empty;
    public string ContactUsPhoneNumber { get; set; } = string.Empty;
    public string ContactUsEmail { get; set; } = string.Empty;
    public bool? IsApprovedByRegulator { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public bool HasLocations { get; set; }
    public bool HasOnlineDeliveryOption { get; set; }
}
