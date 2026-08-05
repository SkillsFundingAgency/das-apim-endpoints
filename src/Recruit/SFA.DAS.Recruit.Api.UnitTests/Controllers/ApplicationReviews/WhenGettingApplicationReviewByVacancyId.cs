using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews
{
    [TestFixture]
    public class WhenGettingApplicationReviewByVacancyId
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            Guid vacancyId,
            GetApplicationReviewsByVacancyIdQueryResult mediatorResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApplicationReviewsByVacancyIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var actual = await controller.GetApplicationReviewsByVacancyId(vacancyId, CancellationToken.None) as OkObjectResult;
            actual.Should().NotBeNull();
            mockMediator.Verify(x => x.Send(It.Is<GetApplicationReviewsByVacancyIdQuery>(
                c => c.VacancyId == vacancyId), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid vacancyId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApplicationReviewsByVacancyIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var actual = await controller.GetApplicationReviewsByVacancyId(vacancyId, CancellationToken.None) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}