using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.SharedOuterApi.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            return ((dateTime.Month >= 4) ? dateTime.ToString("yyyy"): dateTime.AddYears(-1).ToString("yyyy"));
        }
    }
}
