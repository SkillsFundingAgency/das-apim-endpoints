using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetOverallAchievementRateRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(string baseUrl, string sectorSubjectArea)
        {
            var actual = new GetOverallAchievementRateRequest(sectorSubjectArea) {BaseUrl = baseUrl};

            actual.GetUrl.Should().Be($"{baseUrl}api/AchievementRates/Overall?sector={sectorSubjectArea}");

        }
    }
}