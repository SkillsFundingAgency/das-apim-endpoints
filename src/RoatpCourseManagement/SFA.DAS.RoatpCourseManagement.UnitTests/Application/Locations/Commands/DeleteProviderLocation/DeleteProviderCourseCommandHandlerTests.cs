using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Commands.DeleteProviderLocation
{
    [TestFixture]
    public class DeleteProviderLocationCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokedDeleteOnApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            DeleteProviderLocationCommandHandler sut,
            DeleteProviderLocationCommand command)
        {
            var response = new ApiResponse<Unit>(new Unit(), HttpStatusCode.NoContent, null);
            apiClientMock.Setup(c => c.DeleteWithResponseCode<Unit>(It.IsAny<DeleteProviderLocationRequest>(), false))
                .ReturnsAsync(response);

            await sut.Handle(command, new CancellationToken());

            apiClientMock.Verify(c => c.DeleteWithResponseCode<Unit>(It.Is<DeleteProviderLocationRequest>(r => r.UserId == command.UserId && r.Ukprn == command.Ukprn && r.Id == command.Id), false), Times.Once);
            apiClientMock.Verify(x =>
                x.DeleteWithResponseCode<Unit>(It.Is<DeleteProviderLocationRequest>(c =>
                    c.DeleteUrl.Equals($"/providers/{command.Ukprn}/locations/{command.Id}?userId={HttpUtility.UrlEncode(command.UserId)}&userDisplayName={HttpUtility.UrlEncode(command.UserDisplayName)}")), false), Times.Once);
        }
    }
}
