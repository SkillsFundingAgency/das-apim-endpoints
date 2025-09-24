using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.LearnerDataJobs.Api.Controllers;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.Api.UnitTests.Controllers;

public class AddingNewLearnerDataControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Posts_Valid_Learner_With_No_Issues(
        long providerId,
        LearnerDataRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {

        mediator.Setup(x => x.Send(It.Is<AddLearnerDataCommand>(p => p.LearnerData == request), CancellationToken.None))
            .ReturnsAsync(true);

        var result = await controller.PutLearner(providerId, request) as CreatedResult;

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Posts_Valid_Learner_But_With_Issues(
        long providerId,
        LearnerDataRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {

        mediator.Setup(x => x.Send(It.Is<AddLearnerDataCommand>(p => p.LearnerData == request), CancellationToken.None))
            .ReturnsAsync(false);

        var result = await controller.PutLearner(providerId, request) as StatusCodeResult;

        result.StatusCode.Should().Be((int) HttpStatusCode.FailedDependency);
    }


    [Test, MoqAutoData]
    public async Task Then_Posts_Valid_Learner_But_Exception_Occurs(
        long providerId,
        LearnerDataRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {

        mediator.Setup(x => x.Send(It.Is<AddLearnerDataCommand>(p => p.LearnerData == request), CancellationToken.None))
            .ThrowsAsync(new ApplicationException("Band"));

        var result = await controller.PutLearner(providerId, request) as StatusCodeResult;

        result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
    }
}