using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    public class WhenPuttingUserDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Put_Response(
            string govIdentifier,
            CandidatesNameModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            var actual = await controller.AddDetails(govIdentifier, model) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            mediator.Verify(x => x.Send(It.Is<AddDetailsCommand>(
                c=>c.GovUkIdentifier.Equals(govIdentifier)
                && c.FirstName.Equals(model.FirstName)
                && c.LastName.Equals(model.LastName)
                && c.Email.Equals(model.Email)
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_Exception(
            string govUkIdentifier,
            CandidatesNameModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller
            )
        {
            mediator.Setup(x => x.Send(It.IsAny<AddDetailsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.AddDetails(govUkIdentifier, model) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
    }
}
