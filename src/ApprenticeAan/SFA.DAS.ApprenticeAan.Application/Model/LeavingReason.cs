namespace SFA.DAS.ApprenticeAan.Application.Model;

public class LeavingReason
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
}