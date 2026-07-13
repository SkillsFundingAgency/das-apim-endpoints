using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Admin.Api.Controllers;
using SFA.DAS.Admin.Api.Models.UserActions;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.UserActions
{
    public class WhenCheckingUserActionByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserAction_Is_Returned(
            string code,
            CheckUserActionByCodeRequest request,
            CheckUserActionByCodeResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            var commandResult = new CheckUserActionByCodeResult
            {
                Id = response.Id,
                UserId = response.UserId,
                ActionType = response.ActionType,
                ActionTime = response.ActionTime,
                ActionStatus = response.ActionStatus,
                Uln = response.Uln,
                FamilyName = response.FamilyName,
                GivenNames = response.GivenNames,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType,
                CourseName = response.CourseName,
                AdminActions = response.AdminActions?.ConvertAll(a => new CheckUserActionByCodeResult.AdminActionDetail { Username = a.Username, ActionTime = a.ActionTime, Action = a.Action })
            };

            request.Username = request.Username; 

            mediator
                .Setup(x => x.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync(commandResult);

            // Act
            var actual = await controller.CheckUserActionByCode(code, request) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response);

            mediator.Verify(m => m.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code && q.Username == request.Username), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Command_From_Route_Is_Used_When_Body_Is_Null(
            string code,
            CheckUserActionByCodeResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            CheckUserActionByCodeCommand passedCommand = null;

            var commandResult = new CheckUserActionByCodeResult
            {
                Id = response.Id,
                UserId = response.UserId,
                ActionType = response.ActionType,
                ActionTime = response.ActionTime,
                ActionStatus = response.ActionStatus,
                Uln = response.Uln,
                FamilyName = response.FamilyName,
                GivenNames = response.GivenNames,
                CertificateId = response.CertificateId,
                CertificateType = response.CertificateType,
                CourseName = response.CourseName,
                AdminActions = response.AdminActions?.ConvertAll(a => new CheckUserActionByCodeResult.AdminActionDetail { Username = a.Username, ActionTime = a.ActionTime, Action = a.Action })
            };

            mediator
                .Setup(x => x.Send(It.IsAny<CheckUserActionByCodeCommand>(), CancellationToken.None))
                .Callback<IRequest<CheckUserActionByCodeResult>, CancellationToken>((c, ct) => { passedCommand = c as CheckUserActionByCodeCommand; })
                .ReturnsAsync(commandResult);

            // Act
            var actual = await controller.CheckUserActionByCode(code, null) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            passedCommand.Should().NotBeNull();
            passedCommand.Code.Should().Be(code);

            mediator.Verify(m => m.Send(It.IsAny<CheckUserActionByCodeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Ok_Returned_With_Null_If_Command_Result_Does_Not_Exist(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync((CheckUserActionByCodeResult)null);

            // Act
            var actual = await controller.CheckUserActionByCode(code, new CheckUserActionByCodeRequest()) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeNull();

            mediator.Verify(m => m.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<CheckUserActionByCodeCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CheckUserActionByCode(code, new CheckUserActionByCodeRequest()) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CheckUserActionByCodeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
