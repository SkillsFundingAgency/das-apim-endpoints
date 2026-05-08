using System.Text.RegularExpressions;

namespace SFA.DAS.Aodp.Validation
{
    public static class RegexCache
    {
        public static readonly Regex PersonNameRegex =
            new(ValidationPatterns.Text.PersonName, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

        public static readonly Regex TitleRegex =
            new(ValidationPatterns.Text.Title, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

        public static readonly Regex QanRegex =
            new(ValidationPatterns.Format.QualificationNumber, RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

        public static readonly Regex UkprnRegex =
            new(@"^\d{8}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

    }
}
