namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;

public sealed record Apprenticeship(long ProviderId, int Uln, DateOnly StartDate)
{
    public string Option { get; init; } = "OPTION1";
    public string Standard { get; init; } = "STD";
    public int NumberOfApprenticeships { get; init; } = 1;
    public DateOnly? StopDate { get; init; }
    public long CommitmentsApprenticeshipId { get; init; } = 2;

    internal Apprenticeship WithOption(string option)
        => new Apprenticeship(this) with { Option = option };

    internal Apprenticeship WithMultipleStages(int apprenticeshipCount = 2)
        => new Apprenticeship(this) with { NumberOfApprenticeships = apprenticeshipCount };

    internal Apprenticeship WithStartDate(string date)
        => new Apprenticeship(this) with { StartDate = DateOnly.Parse(date) };

    internal Apprenticeship WithStandard(string standard)
        => new Apprenticeship(this) with { Standard = standard };

    internal Apprenticeship WithCourse(Course course)
        => new Apprenticeship(this) with
        {
            Standard = course.Standard,
            Option = course.FirstOption,
        };
    internal Apprenticeship WithId(int id)
        => new Apprenticeship(this) with { CommitmentsApprenticeshipId = id };
}