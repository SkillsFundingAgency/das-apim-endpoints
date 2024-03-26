using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.PhoneNumber;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenPostingPhoneNumber
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Response(
        string govId,
        CandidatesPhoneNumberModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        var actual = await controller.PhoneNumber(govId, model) as OkObjectResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<CreatePhoneNumberCommand>(
            c => c.GovUkIdentifier.Equals(govId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_Exception(
        string govId,
        CandidatesPhoneNumberModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<CreatePhoneNumberCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.PhoneNumber(govId, model) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
    }
}
