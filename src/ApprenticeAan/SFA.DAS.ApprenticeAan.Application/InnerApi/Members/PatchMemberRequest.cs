namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Members;
public class PatchMemberRequest
{
    public int RegionId { get; set; }
    public string? OrganisationName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set;}
}

