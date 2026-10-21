using System.Net;
using Moq;
using SFA.DAS.RoatpOversight.Application.Commands.AssignCourseTypesToProvider;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.RoatpOversight.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpOversight.UnitTests.Commands.AssignCourseTypesToProvider;

public class AssignCourseTypesToProviderCommandHandlerTests
{
    [Test]
    public async Task Handle_ShouldCallApiClientWithCorrectParameters()
    {
        // Arrange
        var ukprn = 12345678;
        var userId = "userId";
        var userDisplayName = "userDisplayName";
        var command = new AssignCourseTypesToProviderCommand(ukprn, userId, userDisplayName);
        var apiClientMock = new Mock<IRoatpV2ApiClient>();
        apiClientMock
            .Setup(client => client.AddCourseTypes(ukprn, It.IsAny<AddCourseTypesModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
        var handler = new AssignCourseTypesToProviderCommandHandler(apiClientMock.Object);
        // Act
        await handler.Handle(command, CancellationToken.None);
        // Assert
        apiClientMock.Verify(client => client.AddCourseTypes(
            ukprn,
            It.Is<AddCourseTypesModel>(model =>
                model.CourseTypes.Contains(CourseType.Apprenticeship) &&
                model.UserId == userId &&
                model.UserDisplayName == userDisplayName),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
