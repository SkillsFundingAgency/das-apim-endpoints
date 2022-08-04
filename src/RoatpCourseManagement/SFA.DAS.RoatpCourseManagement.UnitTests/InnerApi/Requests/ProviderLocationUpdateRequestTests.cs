using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderLocationUpdateRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderLocationUpdateModel data)
        {
            var request = new ProviderLocationUpdateRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.Id.Should().Be(data.Id);
            request.PutUrl.Should().Be($"providers/{data.Ukprn}/locations/{data.Id}/");
        }
    }
}
