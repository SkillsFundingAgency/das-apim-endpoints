using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ApplicationReviews
{
    [TestFixture]
    public class WhenPostingApplicationReview
    {
        [Test, MoqAutoData]
        public async Task Then_Post_From_Mediator(
            Guid applicationId,
            PostApplicationReviewApiRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {

            var actual = await controller.UpdateApplicationReview(applicationId, request) as NoContentResult;

            actual.Should().NotBeNull();
            mockMediator.Verify(x => x.Send(It.Is<PatchApplicationReviewCommand>(
                c =>
                    c.Id == applicationId
                    && c.Status == request.Status
                    && c.DateSharedWithEmployer == request.DateSharedWithEmployer
                    && c.TemporaryReviewStatus == request.TemporaryReviewStatus
                    && c.HasEverBeenEmployerInterviewing == request.HasEverBeenEmployerInterviewing
                    && c.EmployerFeedback == request.EmployerFeedback
            ), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            Guid applicationId,
            PostApplicationReviewApiRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationReviewsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<PatchApplicationReviewCommand>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var actual = await controller.UpdateApplicationReview(applicationId, request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}