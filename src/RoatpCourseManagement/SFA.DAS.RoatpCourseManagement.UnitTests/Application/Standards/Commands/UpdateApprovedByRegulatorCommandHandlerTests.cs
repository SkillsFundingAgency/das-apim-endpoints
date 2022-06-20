using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
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
        public async Task Handle_CallsApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateApprovedByRegulatorCommandHandler sut,
            UpdateApprovedByRegulatorCommand command,
            CancellationToken cancellationToken,
            UpdateProviderCourseRequest request,
            GetProviderCourseResponse apiResponse)
        {
            apiClientMock.Setup(a => a.Get<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>())).ReturnsAsync(apiResponse);

            apiClientMock.Setup(a => a.PostWithResponseCode<UpdateProviderCourseRequest>(It.IsAny<UpdateProviderCourseRequest>())).ReturnsAsync(new ApiResponse<UpdateProviderCourseRequest>(request, HttpStatusCode.NoContent, string.Empty));

            await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.PostWithResponseCode<UpdateProviderCourseRequest>(It.IsAny<UpdateProviderCourseRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void  Handle_CallsApiClient_ReturnException(
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           UpdateApprovedByRegulatorCommandHandler sut,
           UpdateApprovedByRegulatorCommand command,
           CancellationToken cancellationToken)
        {
            apiClientMock.Setup(a => a.Get<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => sut.Handle(command, cancellationToken));
        }
    }
}
