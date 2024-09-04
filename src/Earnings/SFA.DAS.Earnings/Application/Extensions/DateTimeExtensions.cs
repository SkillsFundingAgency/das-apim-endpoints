namespace SFA.DAS.Earnings.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetNumberOfIncludedCensusDatesUntil(this DateTime start, DateTime end)
        {
            var totalDays = (end - start).Days;
            var includedCensusDateCounter = 0;
            for (var i = 0; i < totalDays; i++)
            {
                if(start.AddDays(i).Day == DateTime.DaysInMonth(start.AddDays(i).Year, start.AddDays(i).Month))
                    includedCensusDateCounter++;
            }
            return includedCensusDateCounter;
        }
    }
}
