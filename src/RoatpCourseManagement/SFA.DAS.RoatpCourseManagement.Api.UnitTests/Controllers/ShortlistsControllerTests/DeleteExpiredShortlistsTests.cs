using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Shortlists.Commands.DeleteExpiredShortlists;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ShortlistsControllerTests;

public class DeleteExpiredShortlistsTests
{
    [Test]
    public async Task DeleteExpiredShortlist_Returns_Accepted()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var controller = new ShortlistsController(mediator.Object);
        // Act
        var result = await controller.DeleteExpiredShortlist();
        // Assert
        mediator.Verify(m => m.Send(It.IsAny<DeleteExpiredShortlistsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<AcceptedResult>();
    }
}
