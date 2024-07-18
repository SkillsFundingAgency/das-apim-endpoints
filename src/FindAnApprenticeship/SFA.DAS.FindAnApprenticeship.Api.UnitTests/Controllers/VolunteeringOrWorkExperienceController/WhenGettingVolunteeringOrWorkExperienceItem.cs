using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperience;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VolunteeringOrWorkExperienceController;
public class WhenGettingVolunteeringOrWorkExperienceItem
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid id,
        Guid applicationId,
        GetVolunteeringOrWorkExperienceItemQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringOrWorkExperienceItemQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId && q.Id == id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetDelete(applicationId, candidateId, id);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetVolunteeringOrWorkExperienceItemApiResponse;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo((GetVolunteeringOrWorkExperienceItemApiResponse)queryResult);
    }


    [Test, MoqAutoData]
    public async Task Then_Mediator_Returns_Null_Response_So_NotFound_Is_Returned(
        Guid candidateId,
        Guid id,
        Guid applicationId,
        GetVolunteeringOrWorkExperienceItemQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringOrWorkExperienceItemQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId && q.Id == id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetDelete(applicationId, candidateId, id);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        Guid id,
        Guid applicationId,
        GetVolunteeringOrWorkExperienceItemQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringOrWorkExperienceItemQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId && q.Id == id),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.GetDelete(applicationId, candidateId, id);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
