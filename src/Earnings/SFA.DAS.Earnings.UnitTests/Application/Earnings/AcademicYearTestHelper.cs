namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public static class AcademicYearTestHelper
{
    public static bool IsEarlierThan(this short academicYear, short comparisonYear)
    {
        return academicYear / 100 < comparisonYear / 100;
    }
}