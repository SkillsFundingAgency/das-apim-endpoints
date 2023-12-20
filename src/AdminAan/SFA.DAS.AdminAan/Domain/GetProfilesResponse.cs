namespace SFA.DAS.AdminAan.Domain;

public class GetProfilesResponse
{
    public IEnumerable<Profile> Profiles { get; set; } = Enumerable.Empty<Profile>();
}

public record Profile(long Id, string Description, string Category, int Ordering);
