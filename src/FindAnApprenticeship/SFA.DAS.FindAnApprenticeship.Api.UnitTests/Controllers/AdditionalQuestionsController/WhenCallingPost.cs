using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
using System.Threading;
using FluentAssertions;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.AdditionalQuestionsController;
public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
    Guid applicationId,
    UpdateAdditionalQuestionQueryResult commandResult,
    PostAdditionalQuestionApiRequest apiRequest,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Api.Controllers.AdditionalQuestionsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateAdditionalQuestionCommand>(c =>
            c.CandidateId.Equals(apiRequest.CandidateId)
            && c.ApplicationId == applicationId),
            CancellationToken.None))
            .ReturnsAsync(commandResult);

        var actual = await controller.Post(applicationId, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<CreatedResult>();
            actual.As<CreatedResult>().Value.Should().BeEquivalentTo((PostAdditionalQuestionApiResponse)commandResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Command_Response_Is_Null_Then_NotFound_Returned(
        Guid applicationId,
        PostAdditionalQuestionApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.AdditionalQuestionsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateAdditionalQuestionCommand>(c =>
            c.CandidateId.Equals(apiRequest.CandidateId)
            && c.ApplicationId == applicationId),
            CancellationToken.None))
            .ReturnsAsync(() => null);

        var actual = await controller.Post(applicationId, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<NotFoundResult>();
        }
    }

    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
        Guid applicationId,
        PostAdditionalQuestionApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.AdditionalQuestionsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpdateAdditionalQuestionCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

        var actual = await controller.Post(applicationId, apiRequest) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
