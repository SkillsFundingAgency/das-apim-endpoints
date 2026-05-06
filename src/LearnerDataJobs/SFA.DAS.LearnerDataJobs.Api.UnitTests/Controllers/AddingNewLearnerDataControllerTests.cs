using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.LearnerDataJobs.Api.Controllers;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Application.Queries;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.LearnerDataJobs.Responses;
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

public class GetAllLearnersControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_All_Learners_Successfully(
        int page,
        int pageSize,
        bool excludeApproved,
        GetAllLearnersResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == pageSize && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetAllLearners(page, pageSize, excludeApproved) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(response);
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == pageSize && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_All_Learners_With_Default_Parameters(
        GetAllLearnersResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == 1 && 
            q.PageSize == 100 && 
            q.ExcludeApproved == true), CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetAllLearners() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(response);
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == 1 && 
            q.PageSize == 100 && 
            q.ExcludeApproved == true), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_When_PageSize_Exceeds_1000(
        int page,
        int pageSize,
        bool excludeApproved,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        // Arrange
        pageSize = 1001;

        // Act
        var result = await controller.GetAllLearners(page, pageSize, excludeApproved) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Value.Should().Be("Page size cannot exceed 1000");
        mediator.Verify(x => x.Send(It.IsAny<GetAllLearnersQuery>(), CancellationToken.None), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_When_PageSize_Is_Exactly_1000(
        int page,
        bool excludeApproved,
        GetAllLearnersResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        // Arrange
        var pageSize = 1000; 
        mediator.Setup(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == pageSize && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetAllLearners(page, pageSize, excludeApproved) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(response);
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == pageSize && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Handles_Default_PageSize_Successfully(
        int page,
        bool excludeApproved,
        GetAllLearnersResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] LearnersController controller)
    {
        // Arrange
        mediator.Setup(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == 100 && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetAllLearners(page, excludeApproved: excludeApproved) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(response);
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        mediator.Verify(x => x.Send(It.Is<GetAllLearnersQuery>(q => 
            q.Page == page && 
            q.PageSize == 100 && 
            q.ExcludeApproved == excludeApproved), CancellationToken.None), Times.Once);
    }
}