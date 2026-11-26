namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverSectorSubjectArea> SectorSubjectAreas { get; set; } = [];
}

public class RolloverSectorSubjectArea
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}
