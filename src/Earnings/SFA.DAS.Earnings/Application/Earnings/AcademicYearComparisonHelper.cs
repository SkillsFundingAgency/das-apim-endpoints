namespace SFA.DAS.Earnings.Application.Earnings;

public static class AcademicYearComparisonHelper
{
    public static bool IsEarlierThan(this short academicYear, short comparisonYear)
    {
        return short.Parse($"{academicYear}"[..2]) < short.Parse($"{comparisonYear}"[..2]);
    }
}