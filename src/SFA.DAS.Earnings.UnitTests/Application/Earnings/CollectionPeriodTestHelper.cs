namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public static class CollectionPeriodTestHelper
{
    public static DateTime GetCensusDateForCollectionPeriod(short academicYear, byte collectionPeriod)
    {
        int year;
        int month;
        if (collectionPeriod < 6)
        {
            year = academicYear / 100;
            month = collectionPeriod + 7;
        }
        else
        {
            year = (academicYear / 100) + 1;
            month = collectionPeriod - 5;
        }

        var day = DateTime.DaysInMonth(year, month);
        return new DateTime(year, month, day);
    }
}