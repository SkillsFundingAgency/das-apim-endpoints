namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public sealed record RolloverQueryBuilderRequest : IQueryBuilderFilterRequest
{
    public IReadOnlyCollection<int> LevelIds { get; init; } = [];
    public IReadOnlyCollection<int> TypeIds { get; init; } = [];
    public IReadOnlyCollection<string> SectorSubjectAreaIds { get; init; } = [];
    public IReadOnlyCollection<string> AwardingOrganisationIds { get; init; } = [];
}

public sealed record RolloverQueryBuilderTypesRequest(IReadOnlyCollection<int> LevelIds) : IQueryBuilderFilterRequest;

public sealed record RolloverQueryBuilderSectorSubjectAreaRequest(IReadOnlyCollection<int> LevelIds, IReadOnlyCollection<int> TypeIds) : IQueryBuilderFilterRequest;

public sealed record RolloverQueryBuilderAwardingOrganisationsRequest(IReadOnlyCollection<int> LevelIds, IReadOnlyCollection<int> TypeIds, IReadOnlyCollection<string> SectorSubjectAreaIds) : IQueryBuilderFilterRequest;

public interface IQueryBuilderFilterRequest;