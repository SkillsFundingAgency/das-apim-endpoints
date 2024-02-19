using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using FluentAssertions;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VolunteeringOrWorkExperienceController;
public class WhenGettingDeleteVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid id,
        Guid applicationId,
        GetDeleteVolunteeringOrWorkExperienceQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteVolunteeringOrWorkExperienceQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId && q.Id == id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetDeleteVolunteeringOrWorkExperience(applicationId, candidateId, id);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetDeleteVolunteeringOrWorkHistoryApiResponse;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo((GetDeleteVolunteeringOrWorkHistoryApiResponse)queryResult);
    }
}
