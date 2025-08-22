using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Events;

[TestFixture]
internal class WhenPostingApplicationReviewSharedNotification
{

    [Test, MoqAutoData]
    public async Task Then_Post_From_Mediator(
        PostApplicationReviewSharedNotificationApiRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EventsController controller)
    {

        var actual = await controller.SendApplicationReviewSharedNotification(request) as NoContentResult;

        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<ApplicationReviewSharedCommand>(
            c =>
                c.HashAccountId == request.HashAccountId
                && c.VacancyId == request.VacancyId
                && c.ApplicationId == request.ApplicationId
                && c.TrainingProvider == request.TrainingProvider
                && c.AdvertTitle == request.AdvertTitle
                && c.VacancyReference == request.VacancyReference
        ), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        PostApplicationReviewSharedNotificationApiRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EventsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<ApplicationReviewSharedCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var actual = await controller.SendApplicationReviewSharedNotification(request) as StatusCodeResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}