namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;

public class GetApprenticeResult
{
    public Guid MemberId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string OrganisationName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int RegionId { get; set; }
}
