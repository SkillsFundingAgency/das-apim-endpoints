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
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;

public class WhenGettingCandidatePostcode
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Postcode_Returned_For_Candidate(
        Guid candidateId,
        GetCandidatePostcodeQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetCandidatePostcodeQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(mediatorResponse);
        
        var actual = await controller.GetCandidatePostcode(candidateId) as OkObjectResult;

        actual.Should().NotBeNull();
        actual!.Value.Should().BeEquivalentTo(new { mediatorResponse.Postcode });
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Postcode_NotFound_Response_Returned(
        Guid candidateId,
        GetCandidatePostcodeQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediatorResponse.Postcode = null;
        mediator.Setup(x =>
                x.Send(It.Is<GetCandidatePostcodeQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(mediatorResponse);
        
        var actual = await controller.GetCandidatePostcode(candidateId) as NotFoundResult;

        actual.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServer_Error_Response_Returned(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<GetCandidatePostcodeQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.GetCandidatePostcode(candidateId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}