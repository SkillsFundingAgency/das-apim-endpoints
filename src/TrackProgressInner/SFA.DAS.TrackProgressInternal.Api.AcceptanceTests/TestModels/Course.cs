namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;

public sealed record Course(string Standard, params string[] Options)
{
    public sealed record Ksb(string Description, Guid Id)
    {
        public Ksb() : this("") { }
        public Ksb(string Type) : this(Type, Guid.NewGuid()) { }
    }

    public IEnumerable<Ksb> Ksbs { get; init; } =
        Faker.Extensions.EnumerableExtensions.Times(3, _ =>
            new Ksb(Faker.Lorem.Words(1).First()));

    public IEnumerable<Guid> KsbIds => Ksbs.Select(x => x.Id);

    internal Course WithStandard(string standard)
        => new Course(this) with { Standard = standard };
    
    internal Course WithKsbs(params Ksb[] ksbs)
        => new Course(this) with { Ksbs = ksbs };

    internal string[] CoreAndOptions => Options.Any() ? Options : new[] { "core" };

    internal string FirstOption => Options.Any() ? Options.First() : "core";
}