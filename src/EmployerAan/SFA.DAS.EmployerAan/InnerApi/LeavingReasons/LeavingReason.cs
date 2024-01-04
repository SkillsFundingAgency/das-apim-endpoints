namespace SFA.DAS.EmployerAan.InnerApi.LeavingReasons;

public class LeavingReason
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
}