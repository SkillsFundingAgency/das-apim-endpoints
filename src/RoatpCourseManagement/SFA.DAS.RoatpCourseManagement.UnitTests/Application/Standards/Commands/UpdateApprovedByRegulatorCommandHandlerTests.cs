using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class UpdateApprovedByRegulatorCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient_ResponseSuccessful(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateApprovedByRegulatorCommandHandler sut,
            UpdateApprovedByRegulatorCommand command,
            CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>(string.Empty, HttpStatusCode.NoContent, string.Empty);
            apiClientMock.Setup(c => c.PatchWithResponseCode(It.IsAny<PatchProviderCourseRequest>())).ReturnsAsync(apiResponse);

            await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.PatchWithResponseCode(It.IsAny<PatchProviderCourseRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient_ResponseNotSuccessful_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateApprovedByRegulatorCommandHandler sut,
            UpdateApprovedByRegulatorCommand command,
            CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>(string.Empty, HttpStatusCode.InternalServerError, string.Empty);
            apiClientMock.Setup(c => c.PatchWithResponseCode(It.IsAny<PatchProviderCourseRequest>())).ReturnsAsync(apiResponse);

            Func<Task> action = () => sut.Handle(command, cancellationToken);

            await action.Should().ThrowAsync<InvalidOperationException>();
            apiClientMock.Verify(a => a.PatchWithResponseCode(It.IsAny<PatchProviderCourseRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void Handle_CallsApiClient_ReturnException(
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           UpdateApprovedByRegulatorCommandHandler sut,
           UpdateApprovedByRegulatorCommand command,
           CancellationToken cancellationToken,
           HttpRequestContentException expectedException)
        {
            apiClientMock.Setup(c => c.PatchWithResponseCode(It.IsAny<PatchProviderCourseRequest>())).Throws(expectedException);

            var actualException = Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(command, cancellationToken));

            actualException.Should().Be(expectedException);
        }
    }
}
