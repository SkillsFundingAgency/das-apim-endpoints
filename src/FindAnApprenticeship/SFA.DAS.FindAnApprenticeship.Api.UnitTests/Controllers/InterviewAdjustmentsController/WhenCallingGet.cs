using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
using System.Threading;
using FluentAssertions;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.InterviewAdjustmentsController;
public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
    Guid candidateId,
    Guid applicationId,
    GetInterviewAdjustmentsQueryResult queryResult,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Api.Controllers.InterviewAdjustmentsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.Get(applicationId, candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetInterviewAdjustmentsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetInterviewAdjustmentsApiResponse)queryResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
    Guid candidateId,
    Guid applicationId,
    GetInterviewAdjustmentsQueryResult queryResult,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] Api.Controllers.InterviewAdjustmentsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetInterviewAdjustmentsQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.Get(applicationId, candidateId);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
