using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderCourseLocationCreateRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(AddProviderCourseLocationCommand data)
        {
            var request = new ProviderCourseLocationCreateRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/locations");
        }
    }
}
