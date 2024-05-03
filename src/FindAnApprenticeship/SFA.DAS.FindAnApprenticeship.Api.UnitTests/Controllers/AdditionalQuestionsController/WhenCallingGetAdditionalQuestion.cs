using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.AdditionalQuestionsController;
public class WhenCallingGetAdditionalQuestion
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
       Guid candidateId,
       Guid applicationId,
       Guid questionId,
       int additionalQuestion,
       GetAdditionalQuestionQueryResult queryResult,
       [Frozen] Mock<IMediator> mediator,
       [Greedy] Api.Controllers.AdditionalQuestionsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAdditionalQuestionQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.Id == questionId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.Get(applicationId, candidateId, questionId, additionalQuestion);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetAdditionalQuestionApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetAdditionalQuestionApiResponse)queryResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
       Guid candidateId,
       Guid applicationId,
       Guid questionId,
       int additionalQuestion,
       [Frozen] Mock<IMediator> mediator,
       [Greedy] Api.Controllers.AdditionalQuestionsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAdditionalQuestionQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.Id == questionId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.Get(applicationId, candidateId, questionId, additionalQuestion);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
