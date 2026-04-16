using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Events;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Events;

public class WhenPostingVacancyRejectedNotification
{
    [Test, MoqAutoData]
    public async Task Then_The_Event_Is_Sent_To_The_Handler(
        PostVacancyRejectedEventModel request,
        [Frozen] Mock<IPublisher> mediator,
        [Greedy] EventsController sut)
    {
        // arrange
        var cts = new CancellationTokenSource();
        VacancyRejectedEvent? capturedEvent = null;
        mediator
            .Setup(x => x.Publish(It.IsAny<VacancyRejectedEvent>(), cts.Token))
            .Callback<INotification, CancellationToken>((x, _) => capturedEvent = x as VacancyRejectedEvent);
        
        // act
        var actual = await sut.OnEmployerRejectedVacancy(request, cts.Token) as NoContentResult;

        // assert
        actual.Should().NotBeNull();
        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(request.VacancyId);
    }
}