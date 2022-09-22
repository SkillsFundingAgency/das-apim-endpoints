using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Services.NationalAchievementRates;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services.NationalAchievementRates
{
    [TestFixture]
    public class NationalAchievementRatesPageParserTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task GetCurrentDownloadFilePath_ValidResponse_ReturnsDownloadFilePath(
            NationalAchievementRatesPageParser sut)
        {
            var result = await sut.GetCurrentDownloadFilePath();

            result.Should().NotBeNullOrEmpty();
        }
    }
}
