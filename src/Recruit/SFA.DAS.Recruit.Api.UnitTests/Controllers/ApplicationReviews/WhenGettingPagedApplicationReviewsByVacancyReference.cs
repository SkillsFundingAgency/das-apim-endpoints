using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetPagedApplicationReviewsByVacancyReference;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews;
[TestFixture]
internal class WhenGettingPagedApplicationReviewsByVacancyReference
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApplicationReviews_From_Mediator(
        long vacancyReference,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        GetPagedApplicationReviewsByVacancyReferenceQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetPagedApplicationReviewsByVacancyReferenceQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var actual = await controller.GetPagedByVacancyReference(vacancyReference, pageNumber, pageSize, sortColumn, isAscending, CancellationToken.None) as OkObjectResult;
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<GetPagedApplicationReviewsByVacancyReferenceQuery>(
            c => c.VacancyReference == vacancyReference), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long vacancyReference,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        GetPagedApplicationReviewsByVacancyReferenceQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetPagedApplicationReviewsByVacancyReferenceQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var actual = await controller.GetPagedByVacancyReference(vacancyReference, pageNumber, pageSize, sortColumn, isAscending, CancellationToken.None) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}