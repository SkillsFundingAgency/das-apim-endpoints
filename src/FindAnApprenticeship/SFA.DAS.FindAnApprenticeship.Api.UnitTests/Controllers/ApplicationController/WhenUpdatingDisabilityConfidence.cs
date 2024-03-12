using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

[TestFixture]
public class WhenUpdatingDisabilityConfidence
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        UpdateDisabilityConfidenceModel model,
        PatchApplicationDisabilityConfidenceCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PatchApplicationDisabilityConfidenceCommand>(c =>
                    c.CandidateId == candidateId &&
                    c.ApplicationId == applicationId &&
                    c.DisabilityConfidenceStatus == model.DisabilityConfidenceModelSectionStatus),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.UpdateDisabilityConfidence(applicationId, candidateId, model, CancellationToken.None);
        var actualObject = ((OkObjectResult)actual).Value as FindAnApprenticeship.Models.Application;

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(result.Application);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Null_NotFound_Returned(
        Guid applicationId,
        Guid candidateId,
        UpdateDisabilityConfidenceModel model,
        PatchApplicationDisabilityConfidenceCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        result.Application = null;
        mediator.Setup(x => x.Send(It.Is<PatchApplicationDisabilityConfidenceCommand>(c =>
                    c.CandidateId == candidateId &&
                    c.ApplicationId == applicationId &&
                    c.DisabilityConfidenceStatus == model.DisabilityConfidenceModelSectionStatus),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.UpdateDisabilityConfidence(applicationId, candidateId, model, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<NotFoundResult>();
        }
    }
}