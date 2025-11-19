using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Api.Models.Users;
using SFA.DAS.ToolsSupport.Application.Commands.EmployerUsers;
using System.Net;
using ApiChangeUserStatusResponse = SFA.DAS.ToolsSupport.Api.Models.Users.ChangeUserStatusResponse;
using InnerChangeUserStatusResponse = SFA.DAS.ToolsSupport.InnerApi.Responses.ChangeUserStatusResponse;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

public class UsersControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Suspend_User_Returns_Ok(
        string identifier,
        ChangeUserStatusRequest request,
        InnerChangeUserStatusResponse responseBody,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(
                It.Is<SuspendEmployerUserCommand>(c =>
                    c.Identifier == identifier &&
                    c.ChangedByEmail == request.ChangedByEmail &&
                    c.ChangedByUserId == request.ChangedByUserId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<InnerChangeUserStatusResponse>(responseBody, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await controller.SuspendUser(identifier, request) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var payload = result.Value as ApiChangeUserStatusResponse;
        payload.Should().NotBeNull();
        payload!.Id.Should().Be(responseBody.Id);
    }

    [Test, MoqAutoData]
    public async Task Then_Suspend_User_Returns_NotFound_When_Api_Returns_NotFound(
        string identifier,
        ChangeUserStatusRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<SuspendEmployerUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<InnerChangeUserStatusResponse>(null, HttpStatusCode.NotFound, string.Empty));

        // Act
        var result = await controller.SuspendUser(identifier, request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_Suspend_User_Returns_Ok_When_Api_Returns_BadRequest_With_Body(
        string identifier,
        ChangeUserStatusRequest request,
        InnerChangeUserStatusResponse responseBody,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<SuspendEmployerUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<InnerChangeUserStatusResponse>(responseBody, HttpStatusCode.BadRequest, "error"));

        // Act
        var result = await controller.SuspendUser(identifier, request) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test, MoqAutoData]
    public async Task Then_Resume_User_Returns_Result_When_Api_Response_Ok(
        string identifier,
        ChangeUserStatusRequest request,
        InnerChangeUserStatusResponse responseBody,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(
                It.IsAny<ResumeEmployerUserCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<InnerChangeUserStatusResponse>(responseBody, HttpStatusCode.OK, string.Empty));

        // Act
        var result = await controller.ResumeUser(identifier, request) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        mediator.Verify(x => x.Send(It.Is<ResumeEmployerUserCommand>(c =>
            c.Identifier == identifier &&
            c.ChangedByEmail == request.ChangedByEmail &&
            c.ChangedByUserId == request.ChangedByUserId), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Then_Resume_User_Returns_NotFound_When_Api_Returns_NotFound(
        string identifier,
        ChangeUserStatusRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UsersController controller)
    {
        // Arrange
        mediator
            .Setup(x => x.Send(It.IsAny<ResumeEmployerUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<InnerChangeUserStatusResponse>(null, HttpStatusCode.NotFound, string.Empty));

        // Act
        var result = await controller.ResumeUser(identifier, request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_Resume_User_Returns_BadRequest_When_ModelState_Invalid(
        string identifier,
        ChangeUserStatusRequest request,
        [Greedy] UsersController controller)
    {
        // Arrange
        controller.ModelState.AddModelError("test", "error");

        // Act
        var result = await controller.ResumeUser(identifier, request) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Suspend_User_Returns_BadRequest_When_ModelState_Invalid(
        string identifier,
        ChangeUserStatusRequest request,
        [Greedy] UsersController controller)
    {
        // Arrange
        controller.ModelState.AddModelError("test", "error");

        // Act
        var result = await controller.SuspendUser(identifier, request) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
    }
}

