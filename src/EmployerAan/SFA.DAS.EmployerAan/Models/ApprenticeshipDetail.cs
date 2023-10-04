namespace SFA.DAS.EmployerAan.Models;

public class Apprenticeship
{
    public int ActiveApprenticesCount { get; set; }
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}
