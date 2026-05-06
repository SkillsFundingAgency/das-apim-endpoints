using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadableString(this TimeSpan timeSpan)
        {
            var parts = new List<string>();

            if (timeSpan.Days > 0)
                parts.Add($"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}");
            if (timeSpan.Hours > 0)
                parts.Add($"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s" : "")}");
            if (timeSpan.Minutes > 0)
                parts.Add($"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s" : "")}");
            if (timeSpan.Seconds > 0)
                parts.Add($"{timeSpan.Seconds} second{(timeSpan.Seconds > 1 ? "s" : "")}");

            return parts.Count > 0 ? string.Join(", ", parts) : "0 seconds";
        }
    }

}
