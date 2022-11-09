namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;

public static class An
{
    public static Apprenticeship Apprenticeship =>
        new Apprenticeship(12345, 1, new DateOnly(2020, 01, 01)).WithCourse(A.Course);
}

public static class A
{
    public static Course Course =>
        new("STD", "Option");
}