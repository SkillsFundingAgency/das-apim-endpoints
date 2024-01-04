namespace SFA.DAS.ApprenticeAan.Application.Models;

public class LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReason> LeavingReasons { get; set; } = null!;
}