namespace SFA.DAS.EmployerAan.Models;

public class Apprenticeship
{
    public string Sector { get; set; } = null!;
    public string Programme { get; set; } = null!;
    public string Level { get; set; } = null!;
    public int ActiveApprenticesCount { get; set; }
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
