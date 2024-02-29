using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationAdditionalQuestion;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

[TestFixture]
public class WhenUpdatingApplicationAdditionalQuestion
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        UpdateApplicationAdditionalQuestionModel model,
        PatchApplicationAdditionalQuestionCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PatchApplicationAdditionalQuestionCommand>(c =>
                    c.CandidateId == candidateId &&
                    c.ApplicationId == applicationId &&
                    c.AdditionalQuestionOne == model.AdditionalQuestionOne &&
                    c.AdditionalQuestionTwo == model.AdditionalQuestionTwo),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.UpdateAdditionalQuestion(applicationId, candidateId, model, CancellationToken.None);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as FindAnApprenticeship.Models.Application;
        actualObject.Should().NotBeNull();
        actualObject.Should().BeEquivalentTo(result.Application);
    }
}