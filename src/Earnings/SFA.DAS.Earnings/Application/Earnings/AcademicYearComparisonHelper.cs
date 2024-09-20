﻿namespace SFA.DAS.Earnings.Application.Earnings;

public static class AcademicYearComparisonHelper
{
    public static bool IsEarlierThan(this short academicYear, short comparisonYear)
    {
        return academicYear / 100 < comparisonYear / 100;
    }
}