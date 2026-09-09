using System.Net;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

public class WhenSubmittingApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Created_Response_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        var actual = await controller.SubmitApplication(applicationId, candidateId) as CreatedResult;

        actual.Should().NotBeNull();
        mediator.Verify(
            x => x.Send(
                It.Is<SubmitApplicationCommand>(c => c.ApplicationId == applicationId && c.CandidateId == candidateId),
                CancellationToken.None), Times.Once);
    }
    
    [Test,MoqAutoData]
    public async Task Then_If_Exception_InternalServerError_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(
            x => x.Send(
                It.Is<SubmitApplicationCommand>(c => c.ApplicationId == applicationId && c.CandidateId == candidateId),
                CancellationToken.None)).ThrowsAsync(new Exception());
        
        var actual = await controller.SubmitApplication(applicationId, candidateId) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}