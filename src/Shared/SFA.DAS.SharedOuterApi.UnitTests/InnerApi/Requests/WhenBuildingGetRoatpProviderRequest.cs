using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetRoatpProviderRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
        {
            var actual = new GetRoatpProviderRequest(ukprn);

            actual.GetUrl.Should().Be($"api/providers/{ukprn}");
        }
    }
}