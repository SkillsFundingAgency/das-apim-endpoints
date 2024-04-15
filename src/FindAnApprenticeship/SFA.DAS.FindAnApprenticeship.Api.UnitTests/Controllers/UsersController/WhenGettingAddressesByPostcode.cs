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
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenGettingAddressesByPostcode
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        string postcode,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressesByPostcodeQuery>(x => x.Postcode == postcode), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.SelectAddress(candidateId, postcode) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        Guid candidateId,
        string postcode,
        GetCandidateAddressesByPostcodeQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateAddressesByPostcodeQuery>(x => x.Postcode == postcode), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.SelectAddress(candidateId, postcode);

        actual.Should().BeOfType<OkObjectResult>();

        var result = ((OkObjectResult)actual).Value as GetCandidateAddressesByPostcodeQueryResult;

        result.Should().NotBeNull();
        result.Uprn.Should().Be(queryResult.Uprn);
        result.Postcode.Should().Be(queryResult.Postcode);
        result.Addresses.Should().BeEquivalentTo(queryResult.Addresses);
    }

}
