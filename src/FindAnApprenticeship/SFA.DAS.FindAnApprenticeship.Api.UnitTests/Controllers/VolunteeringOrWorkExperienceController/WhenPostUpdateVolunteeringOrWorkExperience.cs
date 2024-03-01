using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateVolunteeringOrWorkExperience;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VolunteeringOrWorkExperienceController;

public class WhenPostUpdateVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        Guid id,
        UpdateVolunteeringOrWorkExperienceCommandResult commandResult,
        PostUpdateVolunteeringOrWorkExperienceApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringOrWorkExperienceCommand>(c =>
                    c.Id == id
                    && c.CandidateId.Equals(apiRequest.CandidateId)
                    && c.ApplicationId == applicationId
                    && c.Employer == apiRequest.EmployerName
                    && c.Description == apiRequest.Description
                    && c.StartDate == apiRequest.StartDate
                    && c.EndDate == apiRequest.EndDate),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        var actual = await controller.PostUpdateWorkExperience(applicationId, id, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(commandResult.Id);
        }
    }

    [Test, MoqAutoData]
    public async Task And_CommandResult_Is_Null_Then_Returns_NotFound(
        Guid applicationId,
        Guid id,
        PostUpdateVolunteeringOrWorkExperienceApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringOrWorkExperienceCommand>(c =>
                    c.Id == id
                    && c.CandidateId.Equals(apiRequest.CandidateId)
                    && c.ApplicationId == applicationId
                    && c.Employer == apiRequest.EmployerName
                    && c.Description == apiRequest.Description
                    && c.StartDate == apiRequest.StartDate
                    && c.EndDate == apiRequest.EndDate),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.PostUpdateWorkExperience(applicationId, id, apiRequest);

        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Thrown_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid id,
        PostUpdateVolunteeringOrWorkExperienceApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VolunteeringOrWorkExperienceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringOrWorkExperienceCommand>(c =>
                    c.Id == id
                    && c.CandidateId.Equals(apiRequest.CandidateId)
                    && c.ApplicationId == applicationId
                    && c.Employer == apiRequest.EmployerName
                    && c.Description == apiRequest.Description
                    && c.StartDate == apiRequest.StartDate
                    && c.EndDate == apiRequest.EndDate),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.PostUpdateWorkExperience(applicationId, id, apiRequest) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}