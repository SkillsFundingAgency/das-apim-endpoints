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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.SelectLearners;

public class WhenGettingLearnerForProvider
{
    [Test, MoqAutoData]
    public async Task Then_Get_Returns_Learner_From_Mediator(
        long providerId,
        long learnerId,
        GetLearnerForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnerForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Callback((object o, CancellationToken ct) =>
            {
                var q = (GetLearnerForProviderQuery)o;
                q.ProviderId.Should().Be(providerId);
                q.LearnerId.Should().Be(learnerId);
            })
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetALearner(providerId, learnerId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetLearnerForProviderQueryResult;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_InternalServerError(
        long providerId,
        long learnerId,
        GetLearnerForProviderQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SelectLearnersController controller)
    {
        mockMediator.Setup(mediator => mediator.Send(
                It.IsAny<GetLearnerForProviderQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetALearner(providerId, learnerId) as StatusCodeResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}