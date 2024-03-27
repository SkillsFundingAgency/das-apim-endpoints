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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Address;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenPostingSelectAddress
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Response(
        Guid candidateId,
        CandidatesAddressModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        var actual = await controller.SelectAddress(candidateId, model) as OkObjectResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<CreateAddressCommand>(
            c => c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_Exception(
        Guid candidateId,
        CandidatesAddressModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<CreateAddressCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.SelectAddress(candidateId, model) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
    }
}
