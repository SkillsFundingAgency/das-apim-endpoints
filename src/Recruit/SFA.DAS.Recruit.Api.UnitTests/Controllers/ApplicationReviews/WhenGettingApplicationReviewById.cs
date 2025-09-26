using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews;
[TestFixture]
public class WhenGettingApplicationReviewById
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        Guid applicationReviewId,
        GetApplicationReviewByIdQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var actual = await controller.Get(applicationReviewId, CancellationToken.None) as OkObjectResult;
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<GetApplicationReviewByIdQuery>(
            c => c.ApplicationReviewId == applicationReviewId), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Not_Found_Then_Returns_NotFound(
        Guid applicationReviewId,
        GetApplicationReviewByIdQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationReviewByIdQueryResult());

        var actual = await controller.Get(applicationReviewId, CancellationToken.None) as NotFoundResult;
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }



    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        Guid applicationReviewId,
        GetApplicationReviewByIdQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewByIdQuery>(),
                It.IsAny<CancellationToken>()))
           .Throws<InvalidOperationException>();

        var actual = await controller.Get(applicationReviewId, CancellationToken.None) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
