using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetOverallAchievementRateRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int sectorSubjectAreaTier1)
        {
            var actual = new GetOverallAchievementRateRequest(sectorSubjectAreaTier1);

            actual.GetUrl.Should().Be($"api/AchievementRates/Overall?sectorSubjectAreaTier1Code={sectorSubjectAreaTier1}");

        }
    }
}