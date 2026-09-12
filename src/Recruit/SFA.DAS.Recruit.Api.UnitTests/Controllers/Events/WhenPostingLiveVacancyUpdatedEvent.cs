using System.Threading;
using Esfa.Recruit.Vacancies.Client.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Events;

public class WhenPostingLiveVacancyUpdatedEvent
{
    [Test, MoqAutoData]
    public async Task Event_Should_Be_Published(
        PostLiveVacancyUpdatedEventModel payload,
        [Frozen] Mock<IMessageSession> messageSession,
        [Greedy] EventsController sut)
    {
        // arrange
        LiveVacancyUpdatedEvent? capturedEvent = null;
        messageSession
            .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()))
            .Callback<object, PublishOptions>((x, _) => capturedEvent = x as LiveVacancyUpdatedEvent)
            .Returns(Task.CompletedTask);

        // act
        var result = await sut.OnLiveVacancyUpdated(payload, CancellationToken.None);

        // assert
        result.Should().BeOfType<NoContentResult>();
        capturedEvent.Should().NotBeNull();
        capturedEvent.VacancyId.Should().Be(payload.VacancyId);
        capturedEvent.VacancyReference.Should().Be(payload.VacancyReference);
        capturedEvent.UpdateKind.Should().Be(payload.UpdateKind);
    }
}