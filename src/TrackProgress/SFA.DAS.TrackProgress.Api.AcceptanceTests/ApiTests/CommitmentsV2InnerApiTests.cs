using AutoFixture;
using FluentAssertions;

using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ApiTests
{
    public class CommitmentsV2InnerApiTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void GetApprenticeshipsRequestUrl()
        {
            var providerId = _fixture.Create<long>();
            var uln = _fixture.Create<long>();
            var startDate = _fixture.Create<DateTime>();
            var startDateString = startDate.ToString("yyyy-MM-dd");

            var instance = new GetApprenticeshipsRequest(providerId, uln, startDate);
            instance.GetUrl.Should().Be($"api/apprenticeships?providerid={providerId}&searchterm={uln}&startdate={startDateString}");
        }

        [Test]
        public void GetProviderRequestUrl()
        {
            var providerId = 12345;

            var instance = new GetProviderRequest(providerId);
            instance.GetUrl.Should().Be($"api/providers/12345");
        }
    }
}