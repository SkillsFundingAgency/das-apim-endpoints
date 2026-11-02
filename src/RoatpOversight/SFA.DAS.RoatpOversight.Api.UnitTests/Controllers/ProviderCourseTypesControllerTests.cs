using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.RoatpOversight.Api.Controllers;
using SFA.DAS.RoatpOversight.Application.Commands.AssignCourseTypesToProvider;

namespace SFA.DAS.RoatpOversight.Api.UnitTests.Controllers;

public class ProviderCourseTypesControllerTests
{
    [Test]
    public async Task AddCourseTypes_ReturnsOkResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new ProviderCourseTypesController(mediatorMock.Object);
        var command = new AssignCourseTypesToProviderCommand(12345678, "userId", "userDisplayName");
        // Act
        var result = await controller.AddCourseTypes(command, CancellationToken.None);
        // Assert
        Assert.IsInstanceOf<OkResult>(result);
        mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}
