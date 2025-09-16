namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

internal static class InstalmentHelper
{
    internal static int GetNumberOfInstalmentsBetweenDates(DateTime startDate, DateTime endDate)
    {
        var totalDays = 1 + (endDate - startDate).Days;
        var includedCensusDateCounter = 0;
        for (var i = 0; i < totalDays; i++)
        {
            var dateToCheck = startDate.AddDays(i);
            var daysInMonth = DateTime.DaysInMonth(dateToCheck.Year, dateToCheck.Month);

            if (dateToCheck.Day == daysInMonth)
                includedCensusDateCounter++;
        }
        return includedCensusDateCounter;

    }

}
