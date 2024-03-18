using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.CandidatesController;
public class WhenGettingCandidate
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Get_Response(
        Guid candidateId,
        GetCandidateDetailsQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.CandidatesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateDetailsQuery>(query => query.CandidateId == candidateId),
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        var actual = await controller.Get(candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actual.As<OkObjectResult>().Value.Should().BeOfType<CandidateDetailsApiResponse>();
            actual.As<OkObjectResult>().Value.As<CandidateDetailsApiResponse>().Should().BeEquivalentTo(result);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_NotFound_Response(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.CandidatesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateDetailsQuery>(query => query.CandidateId == candidateId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCandidateDetailsQueryResult)null);

        var actual = await controller.Get(candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<NotFoundResult>();
        }
    }
}
