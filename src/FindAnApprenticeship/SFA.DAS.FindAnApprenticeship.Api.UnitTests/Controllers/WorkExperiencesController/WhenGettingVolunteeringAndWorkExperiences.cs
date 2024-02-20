using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetWorkExperiences;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.WorkExperiencesController;

[TestFixture]
public class WhenGettingVolunteeringAndWorkExperiences
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.WorkExperiencesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetWorkExperiences(applicationId, candidateId);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetVolunteeringAndWorkExperiencesQueryResult;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo((GetVolunteeringAndWorkExperiencesQueryResult)queryResult);
    }


    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Returns_Null_Response_So_NotFound_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.WorkExperiencesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetWorkExperiences(applicationId, candidateId) as StatusCodeResult;
        actual!.StatusCode.Should().Be(404);
    }
}