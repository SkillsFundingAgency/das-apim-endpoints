using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SkillsAndStrengthsController;
public class WhenCallingGetCandidateSkillsAndStrengths
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        GetCandidateSkillsAndStrengthsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SkillsAndStrengthsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateSkillsAndStrengthsQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetCandidateSkillsAndStrengths(applicationId, candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetCandidateSkillsAndStrengthsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetCandidateSkillsAndStrengthsApiResponse)queryResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.SkillsAndStrengthsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidateSkillsAndStrengthsQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.GetExpectedSkillsAndStrengths(applicationId, candidateId);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
