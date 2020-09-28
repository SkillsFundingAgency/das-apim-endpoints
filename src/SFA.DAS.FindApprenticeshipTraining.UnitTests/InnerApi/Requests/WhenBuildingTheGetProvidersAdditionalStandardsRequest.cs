using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersAdditionalStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int providerId)
        {
            var actual = new GetProviderAdditionalStandardsRequest(providerId);
            
            actual.GetUrl.Should().Be($"api/providers/{providerId}/courses");
        }
    }
}