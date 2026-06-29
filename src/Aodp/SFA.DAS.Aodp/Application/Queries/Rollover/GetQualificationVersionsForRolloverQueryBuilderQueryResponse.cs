namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetQualificationVersionsForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverQualificationVersion> QualificationVersions { get; set; } = [];
}

public class RolloverQualificationVersion
{
    public Guid Id { get; set; }
    public string? QualificationReference { get; set; }
    public string? QualificationName { get; set; }
    public Guid AwardingOrganisationId { get; set; }
}
