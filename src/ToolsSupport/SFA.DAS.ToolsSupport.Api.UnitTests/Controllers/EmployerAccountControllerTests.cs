using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Commands.ChangeUserRole;
using SFA.DAS.ToolsSupport.Application.Commands.SupportCreateInvitation;
using SFA.DAS.ToolsSupport.Application.Commands.SupportResendInvitation;
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.Models.Constants;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

public class EmployerAccountControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_Details_From_Mediator(
           long accountId,
           GetEmployerAccountDetailsResult getDetailsResult,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] EmployerAccountController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetEmployerAccountDetailsQuery>(x =>
                   x.AccountId == accountId
                   && x.SelectedField == AccountFieldSelection.EmployerAccount),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getDetailsResult);

        var controllerResult = await controller.GetAccountDetails(accountId, AccountFieldSelection.EmployerAccount) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetEmployerAccountDetailsResponse;

        model.Account.Should().NotBeNull();
        model.Account.Should().BeEquivalentTo(getDetailsResult);
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnOk_WhenRequestIsValid(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<SupportCreateInvitationCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.FullName == request.FullName &&
            cmd.Email == request.Email &&
            cmd.Role == request.Role), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportCreateInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task SendInvitation_ShouldReturnBadRequest_WhenExceptionIsThrown(
        SupportCreateInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportCreateInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.SendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnOk_WhenRequestIsValid(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<SupportResendInvitationCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.Email == request.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportResendInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ResendInvitation_ShouldReturnBadRequest_WhenExceptionIsThrown(
        SupportResendInvitationRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<SupportResendInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.ResendInvitation(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnOk_WhenRequestIsValid(
    ChangeUserRoleRequest request,
    [Frozen] Mock<IMediator> mockMediator,
    [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<ChangeUserRoleCommand>(cmd =>
            cmd.HashedAccountId == request.HashedAccountId &&
            cmd.Email == request.Email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.OK)
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as OkResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenResponseStatusIsNotOk(
        ChangeUserRoleRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<ChangeUserRoleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(HttpStatusCode.BadRequest)
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenExceptionIsThrown(
        ChangeUserRoleRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.IsAny<ChangeUserRoleCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"))
            .Verifiable();

        // Act
        var response = await controller.ChangeUserRole(request) as BadRequestResult;

        // Assert
        mockMediator.Verify();
        response.Should().NotBeNull();
    }
}
