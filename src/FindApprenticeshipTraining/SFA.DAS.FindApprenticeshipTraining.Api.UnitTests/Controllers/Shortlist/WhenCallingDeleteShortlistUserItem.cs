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
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItemForUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist
{
    public class WhenCallingDeleteShortlistUserItem
    {
        [Test, MoqAutoData]
        public async Task Then_Deletes_Shortlist_Item_For_User_From_Mediator(
            Guid shortlistUserId,
            Guid id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            var controllerResult = await controller.DeleteShortlistItemForUser(id,shortlistUserId) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<DeleteShortlistItemForUserCommand>(command =>
                        command.UserId == shortlistUserId 
                        && command.Id == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid shortlistUserId,
            Guid id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<DeleteShortlistItemForUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.DeleteShortlistItemForUser(id,shortlistUserId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}