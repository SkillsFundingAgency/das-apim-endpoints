using System.Globalization;

namespace SFA.DAS.Campaign.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// The hub value is title case in the CMS and it's case sensitive. 
        /// </summary>
        /// <param name="valueToAmend"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string valueToAmend)
        {
            var textInfo = new CultureInfo("en-GB", false).TextInfo;

            return textInfo.ToTitleCase(valueToAmend);
        }
    }
}
