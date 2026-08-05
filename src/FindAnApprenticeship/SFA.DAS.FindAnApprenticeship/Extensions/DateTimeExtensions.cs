using System;

namespace SFA.DAS.FindAnApprenticeship.Extensions;

public static class DateTimeExtensions
{
    public static TimeSpan TimeUntilMinutesPastHour(this DateTime dt, int minutesPastHour)
    {
        var nextExpiry = DateTime.UtcNow.Date.AddHours(dt.Hour + 1).AddMinutes(5);
        var diff = nextExpiry - dt;
        if (diff.TotalMinutes > 60)
        {
            diff = diff.Add(TimeSpan.FromMinutes(-60));
        }

        return diff;
    }
}