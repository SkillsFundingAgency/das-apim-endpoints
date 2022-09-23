using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services.NationalAchievementRates
{
    [TestFixture]
    public class NationalAchievementRatesPageParserTests
    {
        [Ignore("failing test")]
        [Test]
        public async Task GetCurrentDownloadFilePath_ValidResponse_ReturnsDownloadFilePath(
            NationalAchievementRatesPageParser sut)
        {
            var nationalAchievementRatesDownloadPageUrl = "http://test.com";

            var result = await sut.GetCurrentDownloadFilePath(nationalAchievementRatesDownloadPageUrl);

            result.Should().NotBeNullOrEmpty();
        }
    }
}
