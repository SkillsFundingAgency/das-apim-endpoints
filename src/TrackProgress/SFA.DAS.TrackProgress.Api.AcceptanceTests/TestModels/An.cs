namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;

public static class An
{
    public static Apprenticeship Apprenticeship =>
        new(12345, 1, new DateOnly(2020, 01, 01));
}