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
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetVolunteeringOrWorkExperienceItem;

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

        var actual = await controller.Get(applicationId, candidateId, id);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetVolunteeringOrWorkExperienceItemApiResponse;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo((GetVolunteeringOrWorkExperienceItemApiResponse)queryResult);
    }
}
