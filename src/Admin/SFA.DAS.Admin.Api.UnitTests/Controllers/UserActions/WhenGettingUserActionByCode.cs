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
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.UserActions
{
    public class WhenGettingUserActionByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserAction_Is_Returned(
            string code,
            GetUserActionByCodeResponse response,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            var queryResult = new GetUserActionByCodeQueryResult
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
                StandardCode = response.StandardCode,
                AdminActions = response.AdminActions?.ConvertAll(a => new GetUserActionByCodeQueryResult.AdminActionDetail
                {
                    Username = a.Username,
                    ActionTime = a.ActionTime,
                    Action = a.Action
                })
            };

            mediator
                .Setup(x => x.Send(It.Is<GetUserActionByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetUserActionByCode(code) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(response);

            mediator.Verify(m => m.Send(It.Is<GetUserActionByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_If_UserAction_Does_Not_Exist(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetUserActionByCodeQuery>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync((GetUserActionByCodeQueryResult)null);

            // Act
            var actual = await controller.GetUserActionByCode(code) as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<GetUserActionByCodeQuery>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserActionsController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetUserActionByCodeQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetUserActionByCode(code) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetUserActionByCodeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
