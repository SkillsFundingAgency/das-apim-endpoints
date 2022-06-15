using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class UpdateApprovedByRegulatorRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(UpdateApprovedByRegulatorCommand data)
        {
            var request = new UpdateApprovedByRegulatorRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/");
        }
    }
}
