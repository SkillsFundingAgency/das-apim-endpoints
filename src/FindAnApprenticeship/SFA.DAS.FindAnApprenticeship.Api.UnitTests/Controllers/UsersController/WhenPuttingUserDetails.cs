using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    public class WhenPuttingUserDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Put_Response(
            string govIdentifier,
            string firstName,
            string lastName,
            string email,
            [Greedy] Api.Controllers.UsersController controller)
        {
            var actual = await controller.AddDetails(govIdentifier, firstName, lastName, email) as OkObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task Then_Throws_Exception(
            AddDetailsCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller
            )
        {
            mediator.Setup(x => x.Send(It.IsAny<AddDetailsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.AddDetails(command.GovUkIdentifier, command.FirstName, command.LastName, command.Email) as StatusCodeResult;

            Assert.NotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, actual.StatusCode);

        }

    }
}
