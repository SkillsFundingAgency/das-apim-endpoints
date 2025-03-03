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
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Candidates.Queries.GetCandidate;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Candidates;

public class WhenCallingGetCandidateDetail
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Sent_And_The_Candidate_Data_Returned(
        Guid candidateId,
        GetCandidateQueryResult queryResult,
        [Frozen]Mock<IMediator> mediator,
        [Greedy] CandidatesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetCandidateDetail(candidateId) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual!.Value as Candidate;
        actualModel.Should().BeEquivalentTo(queryResult.Candidate);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Returned_From_Mediator_Query_Then_NotFound_Response_Returned(
        Guid candidateId,
        GetCandidateQueryResult queryResult,
        [Frozen]Mock<IMediator> mediator,
        [Greedy] CandidatesController controller)
    {
        queryResult.Candidate = null;
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetCandidateDetail(candidateId) as NotFoundResult;

        actual.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_From_Mediator_Query_Then_InternalServerError_Response_Returned(
        Guid candidateId,
        [Frozen]Mock<IMediator> mediator,
        [Greedy] CandidatesController controller)
    {
        
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.CandidateId == candidateId), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetCandidateDetail(candidateId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}