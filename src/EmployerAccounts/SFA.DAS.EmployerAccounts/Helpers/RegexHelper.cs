using System.Text.RegularExpressions;

namespace SFA.DAS.EmployerAccounts.Helpers
{
    public static partial class RegexHelper
    {
        [GeneratedRegex("^[A-Za-z0-9]{2}[0-9]{6}$", RegexOptions.IgnoreCase, "en-GB")]
        private static partial Regex CompaniesHouseReference();

        public static bool CheckCompaniesHouseReference(string text)
        {
            return CompaniesHouseReference().IsMatch(text);
        }
    }
}
