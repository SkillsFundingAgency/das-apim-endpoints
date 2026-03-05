using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class GetProviderAdditionalStandardsRequestTests
    {
        [Test, AutoData]
        public void Version_WhenRequested_ReturnsApiVersionNumberTwo(int providerId)
        {
            var req = new GetProviderAdditionalStandardsRequest(providerId);
            req.Version.Should().Be(ApiVersionNumber.Two);
        }

        [Test, AutoData]
        public void GetUrl_GivenProviderId_ReturnsExpectedPath(int providerId)
        {
            var req = new GetProviderAdditionalStandardsRequest(providerId);
            req.GetUrl.Should().Be($"api/providers/{providerId}/courses");
        }
    }
}