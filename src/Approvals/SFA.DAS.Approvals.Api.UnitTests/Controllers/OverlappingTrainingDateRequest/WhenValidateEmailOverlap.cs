using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.OverlappingTrainingDateRequest;

public class WhenValidateEmailOverlap
{
    [Test, MoqAutoData]
    public async Task Then_Sends_Query_To_Validate_Uln_Overlap_On_Start_Dat(
        long cohortId,
        string email,
          long draftApprenticeshipId, 
          string startDate,
          string endDate,
          ValidateEmailOverlapQueryResult result,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] OverlappingTrainingDateRequestController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<ValidateEmailOverlapQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var controllerResult = await controller.ValidateEmailOverlap(draftApprenticeshipId,email,cohortId, startDate, endDate) as ObjectResult;

        Assert.That(controllerResult, Is.Not.Null);

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}