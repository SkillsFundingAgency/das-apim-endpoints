using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteWorkExperience;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VolunteeringOrWorkExperienceController;
public class WhenPostingDeleteVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid id,
        Guid applicationId,
        PostDeleteVolunteeringOrWorkExperienceRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PostDeleteVolunteeringOrWorkExperienceCommand>(c =>
                c.CandidateId == request.CandidateId
                && c.ApplicationId == applicationId
                && c.Id == id),
            CancellationToken.None)).ReturnsAsync(new Unit());

        var actual = await controller.PostDelete(applicationId, id, request);

        actual.Should().BeOfType<OkObjectResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
        Guid id,
        Guid applicationId,
        PostDeleteVolunteeringOrWorkExperienceRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<PostDeleteVolunteeringOrWorkExperienceCommand>(),
                CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.PostDelete(applicationId, id, request) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
