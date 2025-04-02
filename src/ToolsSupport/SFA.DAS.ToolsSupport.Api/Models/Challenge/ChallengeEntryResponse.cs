using SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;

namespace SFA.DAS.ToolsSupport.Api.Models.Challenge;

public class ChallengeEntryResponse
{
    public bool IsValid { get; set; }

    public string Id { get; set; } = "";

    public List<int> Characters { get; set; } = [];

    public static explicit operator ChallengeEntryResponse(ChallengeEntryCommandResult source)
    {

        if (source == null) return new ChallengeEntryResponse();

        return new ChallengeEntryResponse
        {
            Id = source.Id,
            IsValid = source.IsValid,
            Characters = source.Characters
        };
    }
}
