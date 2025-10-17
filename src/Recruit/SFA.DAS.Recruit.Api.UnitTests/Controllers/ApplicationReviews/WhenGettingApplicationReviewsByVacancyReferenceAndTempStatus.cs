using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndTempStatus;
using SFA.DAS.Recruit.Enums;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews;
[TestFixture]
internal class WhenGettingApplicationReviewsByVacancyReferenceAndTempStatus
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_From_Mediator(
        long vacancyReference,
        ApplicationReviewStatus status,
        GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewsByVacancyReferenceAndTempStatusQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var actual = await controller.GetByVacancyReferenceAndStatus(vacancyReference, status, CancellationToken.None) as OkObjectResult;
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<GetApplicationReviewsByVacancyReferenceAndTempStatusQuery>(
            c => c.VacancyReference == vacancyReference), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long vacancyReference,
        ApplicationReviewStatus status,
        GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApplicationReviewsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApplicationReviewsByVacancyReferenceAndTempStatusQuery>(),
                It.IsAny<CancellationToken>()))
           .Throws<InvalidOperationException>();

        var actual = await controller.GetByVacancyReferenceAndStatus(vacancyReference, status, CancellationToken.None) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}