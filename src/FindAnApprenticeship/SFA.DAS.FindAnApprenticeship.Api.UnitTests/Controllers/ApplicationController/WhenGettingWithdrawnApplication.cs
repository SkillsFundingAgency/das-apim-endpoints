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
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

public class WhenGettingWithdrawnApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_Response_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        WithdrawApplicationQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<WithdrawApplicationQuery>(c => 
                c.ApplicationId == applicationId
                && c.CandidateId == candidateId),
            CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.WithdrawApplication(applicationId, candidateId) as OkObjectResult;

        actual.Should().NotBeNull();
        var actualModel = actual!.Value as GetWithdrawnApplicationApiResponse;
        actualModel.Should().NotBeNull();
        actualModel.Should().BeEquivalentTo(result);
    }
    [Test, MoqAutoData]
    public async Task Then_If_The_Response_Is_Empty_NotFound_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<WithdrawApplicationQuery>(c => 
                c.ApplicationId == applicationId
                && c.CandidateId == candidateId),
            CancellationToken.None)).ReturnsAsync(new WithdrawApplicationQueryResult());

        var actual = await controller.WithdrawApplication(applicationId, candidateId) as NotFoundResult;

        actual.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Exception_Thrown_InternalServer_Response_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<WithdrawApplicationQuery>(c => 
                c.ApplicationId == applicationId
                && c.CandidateId == candidateId),
            CancellationToken.None)).ThrowsAsync(new Exception());

        var actual = await controller.WithdrawApplication(applicationId, candidateId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}