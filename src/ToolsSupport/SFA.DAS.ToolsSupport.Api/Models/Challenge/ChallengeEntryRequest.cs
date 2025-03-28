﻿namespace SFA.DAS.ToolsSupport.Api.Models.Challenge;

public class ChallengeEntryRequest
{
    public string Id { get; set; } = "";
    public string Challenge1 { get; set; } = "";
    public string Challenge2 { get; set; } = "";
    public string Balance { get; set; } = "";
    public required int FirstCharacterPosition { get; set; }
    public required int SecondCharacterPosition { get; set; }
}
