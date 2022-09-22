using SFA.DAS.RoatpCourseManagement.Services.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpCourseManagement.Services.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtensions
    {
        public static Age ToAge(this string value)
        {
            switch (value.ToLower())
            {
                case "16-18":
                    return Age.SixteenToEighteen;
                case "19-23":
                    return Age.NineteenToTwentyThree;
                case "24+":
                    return Age.TwentyFourPlus;
                case "all age":
                    return Age.AllAges;
                default:
                    return Age.Unknown;
            }
        }

        public static ApprenticeshipLevel ToApprenticeshipLevel(this string value)
        {
            switch (value.ToLower())
            {
                case "2":
                    return ApprenticeshipLevel.Two;
                case "3":
                    return ApprenticeshipLevel.Three;
                case "4+":
                    return ApprenticeshipLevel.FourPlus;
                case "all levels":
                    return ApprenticeshipLevel.AllLevels;
                default:
                    return ApprenticeshipLevel.Unknown;
            }
        }
    }
}