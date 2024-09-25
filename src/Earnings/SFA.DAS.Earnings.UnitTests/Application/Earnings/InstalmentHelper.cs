namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
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

        internal static int InstalmentInThisPeriod(DateTime startDate, DateTime endDate, short academicYear, byte collectionPeriod)
        {
            return 0;
        }
    }
}
