namespace SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
public class ChallengeEntryCommandResult
{
    public bool IsValid { get; set; }

    public string Id { get; set; } = "";

    public List<int> Characters { get; set; } = [];
}
