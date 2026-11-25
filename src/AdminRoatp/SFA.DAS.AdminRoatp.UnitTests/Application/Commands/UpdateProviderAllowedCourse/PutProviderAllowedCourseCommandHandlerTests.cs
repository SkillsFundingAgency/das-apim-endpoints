using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpdateProviderAllowedCourse;

public class PutProviderAllowedCourseCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyApiClientCalled(
    [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
    [Greedy] UpdateProviderAllowedCourseCommandHandler sut,
    UpdateProviderAllowedCourseCommand command)
    {
        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.Put<PutProviderAllowedCourseModel>(It.IsAny<PutProviderAllowedCourseRequest>()), Times.Once);
    }
}
