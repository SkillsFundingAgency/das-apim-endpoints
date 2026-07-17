namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetTypesForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverType> Types { get; set; } = [];
}

public class RolloverType
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
