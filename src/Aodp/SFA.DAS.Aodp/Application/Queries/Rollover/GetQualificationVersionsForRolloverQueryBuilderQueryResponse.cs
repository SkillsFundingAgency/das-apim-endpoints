namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public class GetQualificationVersionsForRolloverQueryBuilderQueryResponse
{
    public IEnumerable<RolloverCandidate> QualificationVersions { get; set; } = [];
}