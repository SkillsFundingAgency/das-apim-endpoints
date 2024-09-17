namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public static class AcademicYearTestHelper
{
    public static bool IsEarlierThan(this short academicYear, short comparisonYear)
    {
        return short.Parse($"{academicYear}"[..2]) < short.Parse($"{comparisonYear}"[..2]);
    }
}