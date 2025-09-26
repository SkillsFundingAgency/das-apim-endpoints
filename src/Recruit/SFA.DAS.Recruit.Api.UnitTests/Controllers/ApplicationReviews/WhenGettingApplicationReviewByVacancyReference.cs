using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews
{
    [TestFixture]
    public class WhenGettingApplicationReviewByVacancyReference
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long vacancyReference,
            GetApplicationReviewsByVacancyReferenceQueryResult mediatorResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApplicationReviewsByVacancyReferenceQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var actual = await controller.GetApplicationReviewsByVacancyReference(vacancyReference, CancellationToken.None) as OkObjectResult;
            actual.Should().NotBeNull();
            mockMediator.Verify(x => x.Send(It.Is<GetApplicationReviewsByVacancyReferenceQuery>(
                c => c.VacancyReference == vacancyReference), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long vacancyReference,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<PatchApplicationReviewCommand>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var actual = await controller.GetApplicationReviewsByVacancyReference(vacancyReference, CancellationToken.None) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
