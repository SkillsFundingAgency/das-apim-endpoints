namespace SFA.DAS.LearnerData.Helpers;

public static class DateTimeHelper
{
    public static DateTime? EarliestOf(params DateTime?[] dates)
    {
        DateTime? earliest = null;

        foreach (var date in dates)
        {
            if (date.HasValue && (earliest is null || date.Value < earliest.Value))
                earliest = date.Value;
        }

        return earliest;
    }
}