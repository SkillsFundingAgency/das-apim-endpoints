using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
public class ChallengeEntryCommand : IRequest<ChallengeEntryCommandResult>
{
    public long AccountId { get; set; }
    public string Id { get; set; } = "";
    public string Challenge1 { get; set; } = "";
    public string Challenge2 { get; set; } = "";
    public string Balance { get; set; } = "";
    public int FirstCharacterPosition { get; set; }
    public int SecondCharacterPosition { get; set; }
}