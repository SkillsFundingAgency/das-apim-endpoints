using System;
using System.Threading;
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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;
public class WhenUpdatingApplicationVolunteeringAndWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
    Guid applicationId,
    Guid candidateId,
    UpdateVolunteeringAndWorkExperienceModel model,
    PatchApplicationVolunteeringAndWorkExperienceCommandResponse result,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PatchApplicationVolunteeringAndWorkExperienceCommand>(c =>
                    c.CandidateId == candidateId &&
                    c.ApplicationId == applicationId &&
                    c.VolunteeringAndWorkExperienceStatus == model.VolunteeringAndWorkExperienceSectionStatus),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.UpdateVolunteeringAndWorkExperience(applicationId, candidateId, model, CancellationToken.None);
        var actualObject = ((OkObjectResult)actual).Value as FindAnApprenticeship.Models.Application;

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(result.Application);
        }
    }
}
