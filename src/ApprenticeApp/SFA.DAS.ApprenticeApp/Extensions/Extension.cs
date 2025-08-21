using System;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Extensions
{
    public static class Extension
    {
        public static int? GetApprenticeshipType(this string value, int? defaultValue = null)
        {
            if (Enum.TryParse(typeof(ApprenticeshipType), value, true, out var result))
            {
                return (int)result;
            }
            return defaultValue;
        }
    }
}