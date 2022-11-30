using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.DeleteProviderCourseLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class DeleteProviderCourseLocationRequestTests
    {
        [Test, AutoData]
        public void ImplicitOperator_ConstructsRequest(DeleteProviderCourseLocationCommand command)
        {
            var request = (DeleteProviderCourseLocationRequest)command;

            request.Ukprn.Should().Be(command.Ukprn);
            request.LarsCode.Should().Be(command.LarsCode);
            request.DeleteUrl.Should().Be($"/providers/{command.Ukprn}/courses/{command.LarsCode}/location/{command.Id}?userId={HttpUtility.UrlEncode(command.UserId)}&userDisplayName={HttpUtility.UrlEncode(command.UserDisplayName)}");
        }
    }
}
