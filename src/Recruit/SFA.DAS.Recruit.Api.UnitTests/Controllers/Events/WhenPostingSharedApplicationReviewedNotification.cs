using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Events;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Events;

[TestFixture]
public class WhenPostingSharedApplicationReviewedNotification
{
    [Test, MoqAutoData]
    public async Task Then_Post_From_Mediator(
        PostSharedApplicationReviewedEventModel request,
        [Frozen] Mock<IPublisher> mockMediator,
        [Greedy] EventsController controller)
    {

        var actual = await controller.OnSharedApplicationReviewed(request) as NoContentResult;

        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Publish(It.Is<SharedApplicationReviewedEvent>(
            c =>
                c.VacancyId == request.VacancyId
                && c.VacancyReference == request.VacancyReference
        ), CancellationToken.None), Times.Once);
    }
}