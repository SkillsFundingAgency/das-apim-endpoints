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
using SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Shortlist
{
    public class WhenCallingDeleteShortlistForUser
    {
        [Test, MoqAutoData]
        public async Task Then_Deletes_Shortlist_For_User_From_Mediator(
            Guid id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            var controllerResult = await controller.DeleteShortlistForUser(id) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<DeleteShortlistForUserCommand>(command =>
                        command.UserId == id),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ShortlistController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<DeleteShortlistForUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.DeleteShortlistForUser(id) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}