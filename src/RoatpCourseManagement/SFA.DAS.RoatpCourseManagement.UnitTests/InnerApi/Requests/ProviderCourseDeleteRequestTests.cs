using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderCourseDeleteRequestTests
    {
        [Test, AutoData]
        public void ImplicitOperator_ConstructsRequest(DeleteProviderCourseCommand command)
        {
            var request = (DeleteProviderCourseRequest)command;

            request.Ukprn.Should().Be(command.Ukprn);
            request.LarsCode.Should().Be(command.LarsCode);
            request.DeleteUrl.Should().Be($"/providers/{command.Ukprn}/courses/{command.LarsCode}/?userId={HttpUtility.UrlEncode(command.UserId)}");
        }
    }
}
