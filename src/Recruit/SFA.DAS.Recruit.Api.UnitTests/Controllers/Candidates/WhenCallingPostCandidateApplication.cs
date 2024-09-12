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
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Candidates;

public class WhenCallingPostCandidateApplication
{
    [Test, MoqAutoData]
    public async Task Then_Request_Is_Handled_And_Command_Called(
        Guid candidateId,
        Guid applicationId,
        PostApplicationFeedbackRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidatesController controller)
    {
        var actual = await controller.SubmitApplicationFeedback(candidateId, applicationId, request) as NoContentResult;

        actual.Should().NotBeNull();
        mediator.Verify(x=>x.Send(It.Is<CandidateApplicationStatusCommand>(
            c=>
                c.ApplicationId == applicationId
                && c.CandidateId == candidateId
                && c.Feedback == request.CandidateFeedback
                && c.Outcome == request.Status
                && c.VacancyReference == request.VacancyReference
                && c.VacancyTitle == request.VacancyTitle
                && c.VacancyEmployerName == request.VacancyEmployerName
                && c.VacancyCity == request.VacancyCity
                && c.VacancyPostcode == request.VacancyPostcode
            ), CancellationToken.None), Times.Once);
    }
    [Test, MoqAutoData]
    public async Task Then_If_Exception_InternalServer_Response_Returned(
        Guid candidateId,
        Guid applicationId,
        PostApplicationFeedbackRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidatesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<CandidateApplicationStatusCommand>(), CancellationToken.None))
            .ThrowsAsync(new Exception());
        
        var actual = await controller.SubmitApplicationFeedback(candidateId, applicationId, request) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}