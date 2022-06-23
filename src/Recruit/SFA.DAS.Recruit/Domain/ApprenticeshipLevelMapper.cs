using System;

namespace SFA.DAS.Recruit.Domain
{
    public static class ApprenticeshipLevelMapper
    {
        public static bool TryRemapFromInt(int value, out ApprenticeshipLevel result)
        {
            switch (value)
            {
                case 5: // Foundation Degree
                    value = (int)ApprenticeshipLevel.Higher;
                    break;

                case 7: // Masters
                    value = (int)ApprenticeshipLevel.Degree;
                    break;
            }
            if (Enum.IsDefined(typeof(ApprenticeshipLevel), value))
            {
                result = (ApprenticeshipLevel)value;
                return true;
            }
            result = ApprenticeshipLevel.Unknown;
            return false;
        }

        public static ApprenticeshipLevel RemapFromInt(int value)
        {
            if (TryRemapFromInt(value, out ApprenticeshipLevel result))
                return result;
            throw new ArgumentException($"Cannot convert from int {value} to {nameof(ApprenticeshipLevel)}");
        }
    }
}