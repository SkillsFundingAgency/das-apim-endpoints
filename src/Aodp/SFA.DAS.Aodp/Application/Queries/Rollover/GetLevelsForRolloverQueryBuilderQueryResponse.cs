namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetLevelsForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverLevel> Levels { get; set; } = [];
}

public class RolloverLevel
{
    public int Id { get; set; }
    public string? Name { get; set; }
}