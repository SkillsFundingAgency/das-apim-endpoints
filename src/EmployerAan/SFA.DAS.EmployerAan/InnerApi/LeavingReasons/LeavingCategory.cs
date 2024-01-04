namespace SFA.DAS.EmployerAan.InnerApi.LeavingReasons;
public class LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReason> LeavingReasons { get; set; } = null!;
}