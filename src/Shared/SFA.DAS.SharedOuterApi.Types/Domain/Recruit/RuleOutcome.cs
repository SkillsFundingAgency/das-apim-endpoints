namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

public class RuleOutcome
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RuleId RuleId { get; set; }
    public int Score { get; set;  }
    public string Narrative { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public IEnumerable<RuleOutcome>? Details { get; set; }
    public string? Data { get; set; }
}