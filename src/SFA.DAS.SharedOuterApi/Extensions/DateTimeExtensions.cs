using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.SharedOuterApi.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfFinancialYear(this DateTime dateTime)
        {
            return dateTime.Month >= 4 ? new DateTime(dateTime.Year, 4, 6) : new DateTime(dateTime.AddYears(-1).Year, 4, 6);
        }
    }
}
