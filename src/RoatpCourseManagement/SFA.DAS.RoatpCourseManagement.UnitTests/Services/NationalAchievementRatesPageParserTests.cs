using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services
{
    [TestFixture]
    public class NationalAchievementRatesPageParserTests
    {
        [Test]
        public async Task GetCurrentDownloadFilePath_ValidResponse_ReturnsDownloadFilePath()
        {
            var sut = new NationalAchievementRatesPageParser();
            
            var result = await sut.GetCurrentDownloadFilePath();

            result.Should().NotBeNullOrEmpty();
        }
    }
}
