namespace SFA.DAS.ApprenticeAan.Application.Apprentices.Queries.GetApprentice;

public class GetApprenticeQueryResult
{
    public Guid MemberId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string OrganisationName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int RegionId { get; set; }
}

