namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class ProviderModel
{
    public int Ukprn { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public DateTime? DateLastStarts { get; set; }
}
