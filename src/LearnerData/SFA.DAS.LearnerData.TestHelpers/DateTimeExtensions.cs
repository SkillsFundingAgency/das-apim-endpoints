namespace SFA.DAS.LearnerData.TestHelpers;

public static class DateTimeExtensions
{
    public static (short AcademicYear, byte Period) ToAcademicYearAndPeriod(this DateTime date)
    {
        var twoDigitCalendarYear = short.Parse(date.Year.ToString().Substring(2, 2));

        return date.Month < 8
            ? (short.Parse($"{twoDigitCalendarYear - 1}{twoDigitCalendarYear}"), (byte)(date.Month + 5))
            : (short.Parse($"{twoDigitCalendarYear}{twoDigitCalendarYear + 1}"), (byte)(date.Month - 7));
    }

    public static IEnumerable<DateTime> Enumerate(
        this DateTime start,
        DateTime end,
        DateIncrement increment)
    {
        if (start > end)
            yield break;

        var current = start;

        while (current <= end)
        {
            yield return current;

            current = increment switch
            {
                DateIncrement.Daily => current.AddDays(1),
                DateIncrement.Weekly => current.AddDays(7),
                DateIncrement.Monthly => current.AddMonths(1),
                DateIncrement.Yearly => current.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(increment))
            };
        }
    }

    public static IEnumerable<DateTime> Enumerate(
        this DateTime start,
        DateTime end,
        DateIncrement increment,
        out int count)
    { 
        var items = start.Enumerate(end, increment); 
        count = items.Count();
        return items;
    }
}

public enum DateIncrement
{
    Daily,
    Weekly,
    Monthly,
    Yearly
}