namespace SFA.DAS.EmployerAan.InnerApi.Standards;
public class GetStandardResponse
{
    public string? Title { get; set; }
    public int Level { get; set; }
    public string? Route { get; set; }
    public StandardVersionDetail? VersionDetail { get; set; }
}