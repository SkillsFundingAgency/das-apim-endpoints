using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;

public sealed record Apprenticeship(long ProviderId, int Uln, DateOnly StartDate)
{
    public DeliveryModel DeliveryModel { get; init; } = DeliveryModel.PortableFlexiJob;
    public ApprenticeshipStatus Status { get; init; } = ApprenticeshipStatus.Live;
    public string Option { get; init; } = "OPTION1";
    public string Standard { get; init; } = "STD";
    public int NumberOfApprenticeships { get; init; } = 1;
    public DateOnly? StopDate { get; init; }

    internal Apprenticeship WithOption(string option)
        => new Apprenticeship(this) with { Option = option };

    internal Apprenticeship WithMultipleStages(int apprenticeshipCount = 2)
        => new Apprenticeship(this) with { NumberOfApprenticeships = apprenticeshipCount };

    internal Apprenticeship WithStartDate(string date)
        => new Apprenticeship(this) with { StartDate = DateOnly.Parse(date) };

    internal Apprenticeship WithDeliveryModel(DeliveryModel deliveryModel)
        => new Apprenticeship(this) with { DeliveryModel = deliveryModel };

    internal Apprenticeship WithNotStarted()
        => new Apprenticeship(this) with { Status = ApprenticeshipStatus.WaitingToStart };

    internal Apprenticeship WithStartAndStopOnSameDay()
        => new Apprenticeship(this) with { Status = ApprenticeshipStatus.Stopped, StopDate = StartDate };
    
    internal Apprenticeship WithStandard(string standard)
        => new Apprenticeship(this) with { Standard = standard };

    internal Apprenticeship WithCourse(Course course)
        => new Apprenticeship(this) with
        {
            Standard = course.Standard,
            Option = course.FirstOption,
        };
}