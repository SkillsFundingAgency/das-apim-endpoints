using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenGettingCandidateAddress
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.UserAddress(candidateId) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        Guid candidateId,
        GetCandidateAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.UserAddress(candidateId);

        actual.Should().BeOfType<OkObjectResult>();
    }
}
