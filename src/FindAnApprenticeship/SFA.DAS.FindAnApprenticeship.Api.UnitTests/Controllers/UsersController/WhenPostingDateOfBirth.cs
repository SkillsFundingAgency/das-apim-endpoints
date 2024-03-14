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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenPostingDateOfBirth
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Put_Response(
        string govUkIdentifier,
        string email,
        DateTime dob,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        var model = new CandidatesDateOfBirthModel
        {
            Email = email,
            DateOfBirth = DateOnly.FromDateTime(dob)
        };
        var actual = await controller.DateOfBirth(govUkIdentifier, model) as OkObjectResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<UpsertDateOfBirthCommand>(
            c => c.GovUkIdentifier.Equals(govUkIdentifier)
            && c.DateOfBirth.Equals(model.DateOfBirth)
            && c.Email.Equals(model.Email)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Throws_Exception(
        string govUkIdentifier,
        string email,
        DateTime dob,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        var model = new CandidatesDateOfBirthModel
        {
            Email = email,
            DateOfBirth = DateOnly.FromDateTime(dob)
        };
        mediator.Setup(x => x.Send(It.IsAny<UpsertDateOfBirthCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.DateOfBirth(govUkIdentifier, model) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
    }
}
