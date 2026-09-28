namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class UpdateProviderRestrictedApprenticeshipModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public DateTime? LastDateStarts { get; set; }
}
