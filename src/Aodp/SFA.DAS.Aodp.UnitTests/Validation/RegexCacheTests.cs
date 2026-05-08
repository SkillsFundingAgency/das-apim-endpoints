using NUnit.Framework;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation
{
    [TestFixture]
    public class RegexCacheTests
    {
        [Test]
        public void Person_name_allows_valid_name()
        {
            var result = RegexCache.PersonNameRegex.IsMatch("John Smith");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Person_name_rejects_invalid()
        {
            var result = RegexCache.PersonNameRegex.IsMatch("John <script>");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Title_regex_allows_valid_title()
        {
            var result = RegexCache.TitleRegex.IsMatch("Project Manager");

            Assert.That(result, Is.True);
        }
    }
}