using System;
using System.Globalization;

namespace SFA.DAS.ApprenticeCommitments.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIsoDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
    }
}
