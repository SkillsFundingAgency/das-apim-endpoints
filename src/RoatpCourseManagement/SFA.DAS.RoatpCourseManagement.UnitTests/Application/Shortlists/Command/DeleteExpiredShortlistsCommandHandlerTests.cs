using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Shortlists.Commands.DeleteExpiredShortlists;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Shortlists.Command;

public class DeleteExpiredShortlistsCommandHandlerTests
{
    [Test]
    public async Task Handle_WhenHandlingDeleteExpiredShortlistsCommand_ThenShouldCallCourseManagementApiClientDeleteMethod()
    {
        // Arrange
        var courseManagementApiClient = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        var sut = new DeleteExpiredShortlistsCommandHandler(courseManagementApiClient.Object);
        var command = new DeleteExpiredShortlistsCommand();
        // Act
        await sut.Handle(command, CancellationToken.None);
        // Assert
        courseManagementApiClient.Verify(x => x.Delete(It.IsAny<DeleteExpiredShortlistsRequest>()), Times.Once);
    }
}
