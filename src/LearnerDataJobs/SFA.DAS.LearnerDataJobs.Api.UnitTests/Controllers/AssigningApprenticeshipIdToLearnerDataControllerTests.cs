using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.LearnerDataJobs.Api.Controllers;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerDataJobs.Api.UnitTests.Controllers;

public class AssigningApprenticeshipIdToLearnerDataControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Patches_Valid_ApprenticeshipId_With_No_Issues(
        long providerId,
        long learnerDataId,
        LearnerDataApprenticeshipIdRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        mediator.Setup(x =>
                x.Send(
                    It.Is<AssignApprenticeshipIdCommand>(p =>
                        p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == request),
                    CancellationToken.None))
            .ReturnsAsync(true);

        var result = await controller.PatchLearnerDataApprenticeshipId(providerId, learnerDataId, request) as OkResult;

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Patches_Valid_ApprenticeshipId_But_With_Issues(
        long providerId,
        long learnerDataId,
        LearnerDataApprenticeshipIdRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        mediator.Setup(x =>
                x.Send(
                    It.Is<AssignApprenticeshipIdCommand>(p =>
                        p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == request),
                    CancellationToken.None))
            .ReturnsAsync(false);

        var result = await controller.PatchLearnerDataApprenticeshipId(providerId, learnerDataId, request) as StatusCodeResult;

        result.StatusCode.Should().Be((int) HttpStatusCode.FailedDependency);
    }


    [Test, MoqAutoData]
    public async Task Then_Patches_Valid_ApprenticeshipId_But_Exception_Occurs(
        long providerId,
        long learnerDataId,
        LearnerDataApprenticeshipIdRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {

        mediator.Setup(x =>
                x.Send(
                    It.Is<AssignApprenticeshipIdCommand>(p =>
                        p.ProviderId == providerId && p.LearnerDataId == learnerDataId && p.PatchRequest == request),
                    CancellationToken.None))
            .ThrowsAsync(new ApplicationException("Band"));

        var result = await controller.PatchLearnerDataApprenticeshipId(providerId, learnerDataId, request) as StatusCodeResult;

        result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
    }   
}