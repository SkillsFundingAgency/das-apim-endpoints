using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderCourseCreateRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(CreateProviderCourseCommand data)
        {
            var request = new ProviderCourseCreateRequest(data);

            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/");
            request.Data.Should().Be(data);
        }
    }
}
