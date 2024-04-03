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
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenGettingCandidateName
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        string govIdentifier,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateNameQuery>(x => x.GovUkIdentifier == govIdentifier), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.UserName(govIdentifier) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        string govIdentifier,
        GetCandidateNameQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateNameQuery>(x => x.GovUkIdentifier == govIdentifier), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.UserName(govIdentifier);

        actual.Should().BeOfType<OkObjectResult>();
    }
}
