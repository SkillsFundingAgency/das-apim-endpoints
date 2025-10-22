using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews;
[TestFixture]
internal class WhenGettingManyByApplicationIds
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        GetManyApplicationReviewsApiRequest request,
        GetApplicationReviewsByIdsQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewsByIdsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var actual = await controller.GetManyByApplicationIds(request, CancellationToken.None) as OkObjectResult;
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<GetApplicationReviewsByIdsQuery>(
            c => c.ApplicationIds == request.ApplicationReviewIds), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        GetManyApplicationReviewsApiRequest request,
        GetApplicationReviewsByIdsQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewsByIdsQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var actual = await controller.GetManyByApplicationIds(request, CancellationToken.None) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
