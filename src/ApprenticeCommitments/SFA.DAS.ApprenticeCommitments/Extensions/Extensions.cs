using System;
using SFA.DAS.ApprenticeCommitments.Types;
using System.Globalization;

namespace SFA.DAS.ApprenticeCommitments.Extensions
{
    public static class Extensions
    {
        public static string ToIsoDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }
        
        public static int GetApprenticeshipType(this string value, int defaultValue = 0)
        {
            if (Enum.TryParse(typeof(ApprenticeshipType), value, true, out var result))
            {
                return (int)result;
            }
            return defaultValue;
        }
    }
}
