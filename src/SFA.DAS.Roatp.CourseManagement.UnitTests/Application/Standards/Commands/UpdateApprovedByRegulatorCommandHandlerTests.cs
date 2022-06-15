using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateApprovedByRegulator;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.Application.Standards.Commands
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
            GetProviderCourseRequest getRequest,
            UpdateApprovedByRegulatorRequest request)
        {
            apiClientMock.Setup(a => a.GetWithResponseCode<GetProviderCourseRequest>(It.IsAny<GetProviderCourseRequest>())).ReturnsAsync(new ApiResponse<GetProviderCourseRequest>(getRequest, HttpStatusCode.NoContent, string.Empty));

            apiClientMock.Setup(a => a.PostWithResponseCode<UpdateApprovedByRegulatorRequest>(It.IsAny<UpdateApprovedByRegulatorRequest>())).ReturnsAsync(new ApiResponse<UpdateApprovedByRegulatorRequest>(request, HttpStatusCode.NoContent, string.Empty));

            await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.PostWithResponseCode<UpdateApprovedByRegulatorRequest>(It.IsAny<UpdateApprovedByRegulatorRequest>()), Times.Once);
        }
    }
}
