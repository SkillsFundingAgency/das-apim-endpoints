namespace SFA.DAS.Apprenticeships.Extensions;

public static class DateOfBirthExtensions
{
    public static DateTime GetLastFridayInJuneOfSchoolYearApprenticeTurned16(this DateTime dateOfBirth)
    {
        var sixteenthBirthday = dateOfBirth.AddYears(16);
        var year = sixteenthBirthday.Month > 8 ? sixteenthBirthday.Year + 1 : sixteenthBirthday.Year;

        var lastFriday = new DateTime(year, 6, DateTime.DaysInMonth(year, 6));

        while (lastFriday.DayOfWeek != DayOfWeek.Friday)
            lastFriday = lastFriday.AddDays(-1);

        return lastFriday;
    }
}