using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpdateProviderCourseRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderCourseUpdateModel data)
        {
            var request = new UpdateProviderCourseRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/");
        }
    }
}
