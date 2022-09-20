using AutoFixture;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;

public sealed record Course(string Standard, params string[] Options)
{
    public sealed record Ksb(string Type, Guid Id);

    private static readonly Fixture _fixture = new();

    public IEnumerable<Ksb> Ksbs { get; init; } = _fixture.CreateMany<Ksb>();

    internal Course WithStandard(string standard)
        => new Course(this) with { Standard = standard };

    internal Course WithOptions(params string[] options)
        => new Course(this) with { Options = options };

    internal Course WithoutOptions()
        => new Course(this) with { Options = Array.Empty<string>() };
}