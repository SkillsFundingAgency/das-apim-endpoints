using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.Earnings.Application.Extensions;

public static class DateTimeExtensions
{
    public static int GetNumberOfIncludedCensusDatesUntil(this DateTime start, DateTime end)
    {
        var totalDays = 1 + (end - start).Days;
        var includedCensusDateCounter = 0;
        for (var i = 0; i < totalDays; i++)
        {
            if(start.AddDays(i).Day == DateTime.DaysInMonth(start.AddDays(i).Year, start.AddDays(i).Month))
                includedCensusDateCounter++;
        }
        return includedCensusDateCounter;
    }

    public static DateTime GetCensusDateForCollectionPeriod(this string academicYear, byte collectionPeriod)
    {
        int year = short.Parse($"20{academicYear.Substring(0, 2)}"); // converts 2324 to 2023, this will work until 2099
        int month;

        if (collectionPeriod < 6)
        {
            month = collectionPeriod + 7;
        }
        else
        {
            year++;
            month = collectionPeriod - 5;
        }

        var day = DateTime.DaysInMonth(year, month);
        return new DateTime(year, month, day);
    }

    public static DateTime GetCensusDateForCollectionPeriod(this GetAcademicYearsResponse academicYearToCheck, byte collectionPeriod)
    {
        return academicYearToCheck.AcademicYear.GetCensusDateForCollectionPeriod(collectionPeriod);
    }

    public static bool IsEarlierThan(this short academicYear, short comparisonYear)
    {
        return academicYear / 100 < comparisonYear / 100;
    }

    public static short GetLastYear(this string academicYear)
    {
        int firstTwoDigits = int.Parse( academicYear.Substring(0, 2));
        int lastTwoDigits = int.Parse(academicYear.Substring(2, 2));
        firstTwoDigits--;
        lastTwoDigits--;
        return (short)(firstTwoDigits * 100 + lastTwoDigits);
    }

    public static DateTime GetDateTime(this short academicYear, byte deliveryPeriod)
    {
        var calendarYear = ToCalendarYear(academicYear, deliveryPeriod);
        var calendarMonth = ToCalendarMonth(deliveryPeriod);
        return new DateTime(calendarYear, calendarMonth, 1);
    }

    public static short ToCalendarYear(short academicYear, byte deliveryPeriod)
    {
        if (deliveryPeriod >= 6)
            return short.Parse($"20{academicYear.ToString().Substring(2, 2)}");
        else
            return short.Parse($"20{academicYear.ToString().Substring(0, 2)}");
    }

    public static byte ToCalendarMonth(byte deliveryPeriod)
    {
        if (deliveryPeriod >= 6)
            return (byte)(deliveryPeriod - 5);
        else
            return (byte)(deliveryPeriod + 7);
    }

    /// <summary>
    /// Returns the later of the two specified <see cref="DateTime"/> values.
    /// </summary>
    public static DateTime LatestOf(this DateTime first, DateTime second)
    {
        return first > second ? first : second;
    }

    /// <summary>
    /// Returns the earlier of the two specified <see cref="DateTime"/> values.
    /// </summary>
    public static DateTime EarliestOf(this DateTime first, DateTime second)
    {
        return first < second ? first : second;
    }

    /// <summary>
    /// Returns the later of the two specified <see cref="DateTime"/> values.
    /// </summary>
    public static DateTime? LatestOf(this DateTime? first, DateTime? second)
    {
        if (first == null && second == null) return null;
        if (first == null) return second;
        if (second == null) return first;

        return first.Value.LatestOf(second.Value);
    }

    /// <summary>
    /// Returns the earlier of the two specified <see cref="DateTime"/> values.
    /// </summary>
    public static DateTime? EarliestOf(this DateTime? first, DateTime? second)
    {
        if (first == null && second == null) return null;
        if (first == null) return second;
        if (second == null) return first;
        return first.Value.EarliestOf(second.Value);
    }
}
