namespace SFA.DAS.FindApprenticeshipJobs.Domain.Extensions
{
    public static class DateTimeExtension
    {
        public static string GetClosingDate(DateTime closingDate, bool isExternalVacancy = false)
        {
            var timeSuffix = isExternalVacancy ? string.Empty : " at 11:59pm";
            var timeUntilClosing = closingDate.Date - DateTime.UtcNow;
            var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);

            return daysToExpiry switch
            {
                < 0 => $"Closed on {closingDate:dddd d MMMM yyyy}",
                0 => $"Closes today{timeSuffix}",
                1 => $"Closes tomorrow ({closingDate:dddd d MMMM yyyy}{timeSuffix})",
                <= 31 => $"Closes in {daysToExpiry} days ({closingDate:dddd d MMMM yyyy}{timeSuffix})",
                _ => $"Closes on {closingDate:dddd d MMMM yyyy}"
            };
        }
    }
}