using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class GetProviderAdditionalStandardsRequestUrlTests
    {
        [Test, AutoData]
        public void WhenBuildingGetProviderAdditionalStandardsRequest_ReturnsExpectedUrl(int providerId)
        {
            var actual = new GetProviderAdditionalStandardsRequest(providerId);

            actual.GetUrl.Should().Be($"api/providers/{providerId}/courses");
        }
    }
}