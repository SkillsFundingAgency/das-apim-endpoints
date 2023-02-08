using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetProviderRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(int ukprn)
        {
            var request = new GetProviderRequest(ukprn);

            request.Ukprn.Should().Be(ukprn);
            request.GetUrl.Should().Be($"providers/{request.Ukprn}");
        }
    }
}
